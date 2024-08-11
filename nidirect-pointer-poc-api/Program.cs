using System.Reflection;
using System.Text;
using AspNetCoreRateLimit;
using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nidirect_pointer_poc_api.Auth;
using nidirect_pointer_poc_api.StartupConfig;
using nidirect_pointer_poc_infrastructure;
using nidirect_pointer_poc_infrastructure.Data;
using nidirect_pointer_poc_infrastructure.Features.Common;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

// Setup Jwt authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

// Setup KeyVault for production
if (builder.Environment.IsProduction())
{
    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
}

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Setup api key based authentication
builder.Services.AddScoped<ApiKeyAuthFilter>();

// Setup Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "NI Pointer API",
        Description = "A simple api for address lookups for Northern Ireland.",
        Contact = new OpenApiContact
        {
            Name = "Michael Stevenson",
            Email = "wubblyjuggly@gmail.com"
        }
    });

    // Add bearer auth option to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please provide a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Provide comments on endpoints
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

});

// Setup versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

// Setup fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();

// Setup AutoMapper
builder.Services.AddAutoMapper(typeof(IApplicationMarker).Assembly);

// Setup MediatR
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

// Setup services
builder.Services.AddSingleton<IUriService, UriService>();

// Set up database
builder.Services.AddDbContext<PointerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("pointer-api-db-secret")!);
});

// Setup health checks
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("pointer-api-db-secret")!, healthQuery: "SELECT 1;");

builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("api", "/health");
    options.SetEvaluationTimeInSeconds(5);
    options.SetMinimumSecondsBetweenFailureNotifications(10);
}).AddInMemoryStorage();

// Setup Watchdog monitoring
builder.Services.AddWatchDogServices(options =>
{
    options.IsAutoClear = false;
});

//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

// Set up Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCache"];
    options.InstanceName = "Pointer_";
});

builder.Services.AddResponseCaching();

// Setup rate limiting
builder.Services.AddMemoryCache();
builder.AddRateLimitServices();

var app = builder.Build();

app.UseWatchDogExceptionLogger();
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.InjectStylesheet("/css/nidirect.css");
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PointerContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseIpRateLimiting();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseWatchDog(options =>
{
    options.WatchPageUsername = app.Configuration.GetValue<string>("WatchDog:UserName");
    options.WatchPagePassword = app.Configuration.GetValue<string>("WatchDog:Password");
    options.Blacklist = "health";
});

app.MapHealthChecksUI();
await app.RunAsync();