using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Services
{
    public interface IFileStoreService
    {
        Task<string> StoreAsync(string filename, string contents);
        Task<string> StoreAsync(string filename, byte[] contents);
        Task<byte[]> DownloadAsync(string filename);
        Task<string> DownloadAsStringAsync(string filename);
    }

  
}
