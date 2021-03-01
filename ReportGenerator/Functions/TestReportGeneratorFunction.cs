using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReportGenerator.Services;

namespace ReportGenerator
{
    public class TestReportGeneratorFunction
    {
        public IReportGeneratorService reportGeneratorService { get; set; }

        public TestReportGeneratorFunction(IReportGeneratorService reportGeneratorService)
        {
            this.reportGeneratorService = reportGeneratorService;
        }

        [FunctionName("TestReportGeneratorFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var jsonRequest = @"{
                ""fileName"": ""reportFile.xlsx"",
                ""recipientEmail"": ""<USE YOUR EMAIL FOR TESTING>"",
                ""data"": [
                        {
                            ""WorkOrderId"": ""1"",
                            ""Status"": ""Open"",
                            ""LeadDate"": ""1/12/2020 12:00:00 AM"",
                            ""Source"": ""FDAD"",
                            ""SubCategory"": ""Ceramic/Porcelain"",
                            ""State"": ""New Hampshire""
                        },
                        {
                            ""WorkOrdeId"": ""2"",
                            ""Status"": ""Closed"",
                            ""LeadDate"": ""1/9/2021 12:00:00 AM"",
                            ""Source"": ""FXXD"",
                            ""SubCategory"": ""Tile Shower"",
                            ""State"": ""Florida""
                        }
                    ]
                }";

            await reportGeneratorService.GenerateReportAndSendResultAsync(jsonRequest);

            return new OkObjectResult("Request completed");
        }
    }
}
