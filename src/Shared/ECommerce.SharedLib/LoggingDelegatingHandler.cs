using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.SharedLib
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                request.Headers.Add("Request-Id", _httpContextAccessor.HttpContext.TraceIdentifier);

                var response = await base.SendAsync(request, cancellationToken);

                _logger.LogInformation("Http status: {HttpStatus}", response.StatusCode);

                return response;
            }
            catch (Exception ex)
            {
                var error = new StringBuilder("An error occurred while processing your request");
                error.AppendLine(ex.Message);

                _logger.LogError(error.ToString(), ex);

                return new HttpResponseMessage(HttpStatusCode.BadGateway)
                {
                    RequestMessage = request
                };
            }
        }
    }
}