using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ReportGenerator.DatabaseManager;
using ReportGenerator.FileCreator;
using ReportGenerator.MessageSender;
using ReportGenerator.Services;
using ReportGenerator.StorageManager;

[assembly: FunctionsStartup(typeof(ReportGenerator.Startup))]

namespace ReportGenerator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IReportGeneratorService, ReportGeneratorService>();

            builder.Services.AddScoped<IDatabaseManager, CosmosDbManager>();

            builder.Services.AddScoped<IFileCreator, XlsxFileCreator>();

            builder.Services.AddScoped<IStorageManager, BlobStorageManager>();

            builder.Services.AddScoped<IResponseMessageSender, SendGridEmailSender>();
        }
    }
}
