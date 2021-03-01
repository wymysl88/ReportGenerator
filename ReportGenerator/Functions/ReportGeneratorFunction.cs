using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ReportGenerator.Services;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class ReportGeneratorFunction
    {
        public readonly IReportGeneratorService reportGeneratorService;

        public ReportGeneratorFunction(IReportGeneratorService reportGeneratorService)
        {
            this.reportGeneratorService = reportGeneratorService;
        }

        [FunctionName("ReportGeneratorFunction")]
        public async Task Run([ServiceBusTrigger("ReportRequestQueue", Connection = "ReportRequestQueueConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function starting to process message: {myQueueItem}");

            await reportGeneratorService.GenerateReportAndSendResultAsync(myQueueItem);

            log.LogInformation("Request has been completed");
        }
    }
}
