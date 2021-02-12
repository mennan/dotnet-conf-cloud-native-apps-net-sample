using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace ECommerce.SharedLib
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var traceId = GetTraceId(context);
            context.TraceIdentifier = traceId;
            
            using (LogContext.PushProperty("TraceId", traceId))
            {
                return _next.Invoke(context);
            }
        }
        
        private string GetTraceId(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Request-Id", out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}