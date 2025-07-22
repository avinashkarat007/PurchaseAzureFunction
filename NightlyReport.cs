using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseWebHook;

public class NightlyReport
{
    private readonly ILogger _logger;

    public NightlyReport(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<NightlyReport>();
    }

    [Function("NightlyReport")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}