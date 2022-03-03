using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    /// <summary>
    /// Log requests/responses executed while End-to-End test is running.
    /// </summary>
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-message-handlers"/>
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Request started at {DateTime.Now} => '{request.Method}' - '{request.RequestUri}'");

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation($"Response Received at {DateTime.Now} => {(int)response.StatusCode} - {response.ReasonPhrase} - '{response.RequestMessage.RequestUri}'");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request Content: {0}", response.RequestMessage.Content != null ? await response.RequestMessage.Content.ReadAsStringAsync() : string.Empty);

                _logger.LogError("*******************************************************************************************************************************************");
                _logger.LogError($"Request Details: {request.Method} - '{request.RequestUri}' ");

                if (request.Content != null)
                {
                    _logger.LogError($"Content Details: {await request.Content.ReadAsStringAsync()}");
                }

                if (request.Headers.Any())
                {
                    var headers = new StringBuilder();

                    foreach (var header in request.Headers)
                    {
                        headers.AppendLine($"{header.Key}: {header.Value.FirstOrDefault()}");
                    }

                    _logger.LogError($"Headers: {headers}");
                }

                _logger.LogError($"Details: {await response.Content.ReadAsStringAsync()}");

                _logger.LogError("*******************************************************************************************************************************************");
            }

            return response;
        }
    }
}
