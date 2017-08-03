using AzureStorageHomework.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AzureStorageHomework.Services
{
    public interface IBlobService
    {
        Task<BlobViewModel> GetBlobList();
        Task<bool> CreateBlob(IFormFile file1);
        Task<bool> UpdateBlob(IFormFile file1, string name);
        Task<bool> DeleteBlob(string name);
    }
}
