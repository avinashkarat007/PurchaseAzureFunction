using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseWebHook;

public partial class ProcessNewOrder
{
    private readonly ILogger<ProcessNewOrder> _logger;

    public ProcessNewOrder(ILogger<ProcessNewOrder> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ProcessNewOrder))]
    public void Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")] NewOrderMessage message)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {message.customerName} bought {message.productName}");
    }
}