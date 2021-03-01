using ReportGenerator.DatabaseManager;
using ReportGenerator.FileCreator;
using ReportGenerator.Helpers;
using ReportGenerator.MessageSender;
using ReportGenerator.Models;
using ReportGenerator.StorageManager;
using System.Threading.Tasks;

namespace ReportGenerator.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        private readonly IDatabaseManager databaseManager;

        private readonly IFileCreator fileCreator;

        private readonly IStorageManager storageManager;

        private readonly IResponseMessageSender messageSender;

        public ReportGeneratorService(IDatabaseManager databaseManager, IFileCreator fileCreator, IStorageManager storageManager, IResponseMessageSender messageSender)
        {
            this.databaseManager = databaseManager;
            this.fileCreator = fileCreator;
            this.storageManager = storageManager;
            this.messageSender = messageSender;
        }

        public async Task GenerateReportAndSendResultAsync(string reportRequestJson)
        {
            var reportRequest = ReportRequestParser.ParseReportRequest(reportRequestJson);

            await SaveRequestToDatabaseAsync(reportRequest);

            var generatedReportFile = await GenerateReportFileAsync(reportRequest);

            var reportFileLocation = await SaveFileIntoStorageAsync(generatedReportFile, reportRequest.FileName);

            await SendGenerationResultMessage(reportFileLocation, reportRequest.RecipientEmail);
        }

        private async Task SaveRequestToDatabaseAsync(ReportRequest reportRequest)
        {
            await this.databaseManager.SaveToDatabase(reportRequest);
        }

        private async Task<byte[]> GenerateReportFileAsync(ReportRequest reportRequest)
        {
            return await this.fileCreator.GetReportDataAsByteArrayAsync(reportRequest);
        }

        private async Task<string> SaveFileIntoStorageAsync(byte[] reportData, string fileName)
        {
            return await this.storageManager.SaveIntoStorageAsync(reportData, fileName);
        }

        private async Task SendGenerationResultMessage(string fileUrl, string recipient)
        {
            await this.messageSender.SendMessageAsync(fileUrl, recipient);
        }
    }
}
