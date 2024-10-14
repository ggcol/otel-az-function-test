using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace OpenTelemetryTest
{
    public class MyFunction
    {
        private readonly ILogger _logger;

        public MyFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFunction>();
        }

        [Function(nameof(MyFunction))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var guid = Guid.NewGuid();
            _logger.LogInformation("{TimeStamp} - {Id} - I've received an API call.",
                DateTime.UtcNow.ToString("yyyyMMdd-hhmmssfff"), 
                guid);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("This is a response!");
            _logger.LogInformation("{TimeStamp} - {Id} - I've replied an API call", 
                DateTime.UtcNow.ToString("yyyyMMdd-hhmmssfff"),
                guid);

            return response;
        }
    }
}
