using System;
using System.Threading.Tasks;
using azure_storage_api_core.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace azure_storage_api_core.Services
{
    public class AzureStorageService : IStorageService
    {
        private const string blobfolder = "sampleFolder";
        private readonly AzureStorageSettings settings;
        private readonly CloudStorageAccount storageAccount;

        public AzureStorageService(IOptions<AzureStorageSettings> options)
        {
            this.settings = options.Value;

            storageAccount = CloudStorageAccount.Parse(settings.StorageConnectionString);
        }

        public async Task<string> Upload(byte[] fileData, bool securedStorage)
        {
            try
            {
                var blobClinet = storageAccount.CreateCloudBlobClient();

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                string strContainerName = securedStorage ? "secured-white-elephant" : "white-elephant";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = this.GenerateFileName();

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    // cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private string GenerateFileName()
        {
            return $"{DateTime.UtcNow.ToString("MM-dd")}/{Guid.NewGuid().ToString("n")}.jpg";
        }
    }
}