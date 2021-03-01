using System.Threading.Tasks;

namespace ReportGenerator.StorageManager
{
    public interface IStorageManager
    {
        Task<string> SaveIntoStorageAsync(byte[] fileData, string fileName);
    }
}
