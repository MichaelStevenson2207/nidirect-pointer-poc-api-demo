using AspNetCoreRateLimit;

namespace nidirect_pointer_poc_api.StartupConfig
{
    /// <summary>
    ///  Middleware for rate limiting
    /// </summary>
    public static class ServicesConfig
    {
        /// <summary>
        /// Add rate limiting services to the container.
        /// </summary>
        /// <param name="builder"></param>
        public static void AddRateLimitServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<IpRateLimitOptions>(
                builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();
        }
    }
}