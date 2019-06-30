using System.Threading.Tasks;

namespace azure_storage_api_core.Services
{
    public interface IStorageService
    {
        Task<string> Upload(byte[] fileData, bool securedStorage);
    }
}