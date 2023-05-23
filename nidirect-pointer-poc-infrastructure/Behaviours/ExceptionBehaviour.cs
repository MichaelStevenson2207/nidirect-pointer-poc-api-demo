using MediatR;
using Microsoft.Extensions.Logging;

namespace nidirect_pointer_poc_infrastructure.Behaviours
{
    public class ExceptionBehaviour
    {
        public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

            public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                try
                {
                    var response = await next();
                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception {Ex} at {typeof(TResponse).Name} of {typeof(TRequest).Name} at {DateTime.UtcNow:yyyy-MM-dd hh:mm:ss.fff}", ex, typeof(TResponse).Name, typeof(TRequest).Name, DateTime.UtcNow.ToString("yyyy - MM - dd hh: mm:ss.fff"));

                    throw;
                }
            }
        }
    }
}