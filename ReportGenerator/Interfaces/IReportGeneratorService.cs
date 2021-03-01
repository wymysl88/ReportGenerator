using System.Threading.Tasks;

namespace ReportGenerator.Services
{
    public interface IReportGeneratorService
    {
        Task GenerateReportAndSendResultAsync(string reportRequestJson);
    }
}
