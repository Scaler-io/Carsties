using SearchService.Extensions;
using Serilog.Context;

namespace SearchService.Middlewares
{
    public class CorrelationHeaderEnricher
    {
        private const string CorrelationIdLogPropertyName = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationHeaderEnricher(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = GetOrGenerateCorrelationId(context);
            using (LogContext.PushProperty("ThreadId", Environment.CurrentManagedThreadId))
            {
                LogContext.PushProperty(CorrelationIdLogPropertyName, correlationId);
                context.Response.Headers.Add("CorrelationId", correlationId);
                await _next(context);
            }
        }

        private string GetOrGenerateCorrelationId(HttpContext context) => context?.Request.GetRequestHeaderOrdefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");

    }
}
