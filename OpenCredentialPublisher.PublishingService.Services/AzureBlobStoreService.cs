using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.PublishingService.Data;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{

    public class AzureBlobStoreService : IFileStoreService
    {
        public const string BlobContainerName = PublishServiceConstants.ClrFileBlobContainerName;

        private readonly AzureBlobOptions _options;
        public AzureBlobStoreService(IOptions<AzureBlobOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> StoreAsync(string filename, string contents)
        {
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, BlobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);
                await writer.WriteAsync(contents);
                await writer.FlushAsync();
                ms.Position = 0;

                await blob.UploadAsync(ms);
            }

            return filename;
        }

        public async Task<string> StoreAsync(string filename, byte[] contents)
        {
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, BlobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            using (var ms = new MemoryStream(contents, false))
            {
                await blob.UploadAsync(ms);
            }

            return filename;
        }

        public async Task<string> DownloadAsStringAsync(string filename)
        {
            var bytes = await DownloadAsync(filename);

            string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return utfString;
        }

            public async Task<byte[]> DownloadAsync(string filename)
        {
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, BlobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            BlobDownloadInfo download = await blob.DownloadAsync();

            using (var ms = new MemoryStream())
            {
                await download.Content.CopyToAsync(ms);

                return ms.ToArray();
            }

        }

    }

  
}
