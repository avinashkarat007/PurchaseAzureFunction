using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static PurchaseWebHook.ProcessNewOrder;

namespace PurchaseWebHook
{

    public class PurchaseWebHookResponse
    {
        [QueueOutput("myqueue-items", Connection = "AzureWebJobsStorage")]
        public NewOrderMessage? Message { get; set; }

        public HttpResponseData? HttpResponseData { get; set; }
    }

    public class PurchaseWebHook
    {
        private readonly ILogger<PurchaseWebHook> _logger;

        public PurchaseWebHook(ILogger<PurchaseWebHook> logger)
        {
            _logger = logger;
        }

        [Function(nameof(PurchaseWebHook))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var order = req.ReadFromJsonAsync<NewOrderMessage>().Result;

            return new OkObjectResult($" {order.customerName} purchased {order.productName}!");
        }

        [Function(nameof(GetPurchase))]
        public IActionResult GetPurchase([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get the name value from the query string
            var name = req.Query["name"];

            var userAgent = req.Headers["User-Agent"].ToString();

            return new OkObjectResult($"Hi { name }, Welcome to Azure Functions, using {userAgent}!");
        }
    }
}
