using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
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

        [Function("PurchaseWebHook")]
        public async Task<PurchaseWebHookResponse> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing purchase webhook request...");

            var order = await JsonSerializer.DeserializeAsync<NewOrderMessage>(req.Body);

            var response = req.CreateResponse();

            if (order == null)
            {
                _logger.LogError("Invalid order data.");
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("Invalid order data.");
                return new PurchaseWebHookResponse { HttpResponseData = response };
            }

            // Fix: Use the constructor of NewOrderMessage to initialize the object
            var message = new NewOrderMessage(
                productId: order.productId,
                productName: order.productName,
                quantity: order.quantity,
                customerName: order.customerName,
                price: order.price
            );

            response.StatusCode = HttpStatusCode.OK;

            return new PurchaseWebHookResponse
            {
                Message = message,
                HttpResponseData = response
            };
        }

        // This function is used to get the purchase information
        [Function(nameof(GetPurchase))]
        public IActionResult GetPurchase([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get the name value from the query string
            var name = req.Query["name"];

            var userAgent = req.Headers["User-Agent"].ToString();

            return new OkObjectResult($"Hi {name}, Welcome to Azure Functions, using {userAgent}!");
        }
    }
}
