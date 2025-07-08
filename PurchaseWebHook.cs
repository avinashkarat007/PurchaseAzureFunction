using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseWebHook
{
    public class PurchaseWebHook
    {
        private readonly ILogger<PurchaseWebHook> _logger;

        public PurchaseWebHook(ILogger<PurchaseWebHook> logger)
        {
            _logger = logger;
        }

        [Function(nameof(PurchaseWebHook))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Hi Avinash, Welcome to Azure Functions!");
        }
    }
}
