using Microsoft.Azure.Cosmos;
using ReportGenerator.Models;
using System;
using System.Threading.Tasks;

namespace ReportGenerator.DatabaseManager
{
    public class CosmosDbManager : IDatabaseManager
    {
        private readonly Container container;

        public CosmosDbManager()
        {
            var cosmosDbContainerName = Environment.GetEnvironmentVariable("CosmosDbContainerName");
            var cosmosDbName = Environment.GetEnvironmentVariable("CosmosDbName");
            var cosmosDbClientApplicationName = Environment.GetEnvironmentVariable("CosmosDbClientApplicationName");
            var endpointUri = Environment.GetEnvironmentVariable("CosmosDbEndpointUri");
            var primaryKey = Environment.GetEnvironmentVariable("CosmosDbPrimaryKey");

            var cosmosClientOptions = new CosmosClientOptions() { ApplicationName = cosmosDbClientApplicationName };

            var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);
            this.container = cosmosClient.GetContainer(cosmosDbName, cosmosDbContainerName);
        }

        public async Task SaveToDatabase(ReportRequest reportRequest)
        {
            reportRequest.Id = Guid.NewGuid().ToString();
            await this.container.CreateItemAsync(reportRequest);
        }
    }
}
