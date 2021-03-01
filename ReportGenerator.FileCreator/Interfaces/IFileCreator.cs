using ReportGenerator.Models;
using System.Threading.Tasks;

namespace ReportGenerator.FileCreator
{
    public interface IFileCreator
    {
        Task<byte[]> GetReportDataAsByteArrayAsync(ReportRequest reportRequest);
    }
}
