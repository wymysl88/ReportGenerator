using ReportGenerator.Models;

namespace ReportGenerator.Helpers
{
    public static class ReportRequestParser
    {
        public static ReportRequest ParseReportRequest(string requstBody)
        {
            var reportRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportRequest>(requstBody);

            return reportRequest;
        }
    }
}
