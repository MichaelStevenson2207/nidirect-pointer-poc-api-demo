using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace nidirect_pointer_poc_api.Auth
{
    /// <summary>
    /// Filter for api key based authentication
    /// </summary>
    public sealed class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authorize the request
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                    out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key missing");
                return;
            }

            var apiKey = _configuration.GetValue<string>("ApiKey");
            if (apiKey != extractedApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key");
            }
        }
    }
}