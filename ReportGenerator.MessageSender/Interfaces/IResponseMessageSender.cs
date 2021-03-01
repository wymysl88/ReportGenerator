using System.Threading.Tasks;

namespace ReportGenerator.MessageSender
{
    public interface IResponseMessageSender
    {
        Task SendMessageAsync(string fileUrl, string recipient);
    }
}
