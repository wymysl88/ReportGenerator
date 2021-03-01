using ReportGenerator.Models;
using System.Threading.Tasks;

namespace ReportGenerator.DatabaseManager
{
    public interface IDatabaseManager
    {
        Task SaveToDatabase(ReportRequest reportRequest);
    }
}
