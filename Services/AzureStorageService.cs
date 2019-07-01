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

        public string GetSecuredDocumentAddress()
        {

            var blobClinet = storageAccount.CreateCloudBlobClient();

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            string strContainerName = "secured-white-elephant";
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(strContainerName);
            var permissions = container.GetPermissions();
            var sampleBlob = "06-30/94380671993e4d7cb28013a77290d321.jpg";

            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(sampleBlob);
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
            // to construct a shared access policy that is saved to the container's shared access policies.
            SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read
            };

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

            return blob.Uri + sasBlobToken;
        }


        public string GetDocumentAddress()
        {

            var blobClinet = storageAccount.CreateCloudBlobClient();

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            string strContainerName = "white-elephant";
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(strContainerName);
            var sampleBlob = "sampleFolder/716176775f694ab3a942ef05065be30e.jpg";

            string sasBlobToken;
            var permissions = container.GetPermissions();
            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(sampleBlob);
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
            // to construct a shared access policy that is saved to the container's shared access policies.
            SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read
            };

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

            return blob.Uri + sasBlobToken;
        }

        private string GenerateFileName()
        {
            return $"{DateTime.UtcNow.ToString("MM-dd")}/{Guid.NewGuid().ToString("n")}.jpg";
        }
    }
}