using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ReportGenerator.StorageManager
{
    public class BlobStorageManager: IStorageManager
    {
        private readonly BlobContainerClient containerClient;

        public BlobStorageManager()
        {
            var blobStorageContainerName = Environment.GetEnvironmentVariable("BlobStorageContainerName");
            var blobStorageconnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
                   
            var blobServiceClient = new BlobServiceClient(blobStorageconnectionString);

            this.containerClient = blobServiceClient.GetBlobContainerClient(blobStorageContainerName);
        }

        public async Task<string> SaveIntoStorageAsync(byte[] fileData, string fileName)
        {
            try
            {
                var uniqueFileName = GetUniqueFileName(fileName);

                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                using (var stream = new MemoryStream(fileData))
                {
                    await blobClient.UploadAsync(stream);
                }

                var blobUri = GetSasUriForBlob(blobClient);
                return blobUri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR while saving into BlobStorage\n\t{0}\n", ex.Message);
            }

            return string.Empty;
        }

        private string GetUniqueFileName(string fileNameWithExtension)
        {
            var fileName = Path.GetFileNameWithoutExtension(fileNameWithExtension);
            var extension = Path.GetExtension(fileNameWithExtension);
            var uniquePart = Guid.NewGuid().ToString();

            var uniqueFileName = $"{fileName}_{uniquePart}{extension}";

            return uniqueFileName;
        }

        private static Uri GetSasUriForBlob(BlobClient blobClient, string storedPolicyName = null)
        {
            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                var sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}\n", sasUri);

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key credentials to create a service SAS.");
                return null;
            }
        }
    }
}
