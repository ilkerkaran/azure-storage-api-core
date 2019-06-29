using System.Threading.Tasks;
using azure_storage_api_core.Models;
using Microsoft.Azure.Storage;
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
        }

        public Task<bool> Upload(byte[] fileData)
        {
            throw new System.NotImplementedException();
        }
    }
}