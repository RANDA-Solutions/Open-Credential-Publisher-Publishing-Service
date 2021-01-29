using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class PublishMessageHandlerBase
    {
        private readonly OcpDbContext _context;
        private readonly BlobLeaseService _blobLeaseService;
        protected readonly ILogger<PublishMessageHandlerBase> Log;

        public PublishMessageHandlerBase(IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, ILogger<PublishMessageHandlerBase> log)
        {
            _context = context;
            _blobLeaseService = new BlobLeaseService(blobOptions);
            Log = log;
        }

        protected async Task<PublishRequest> GetPublishRequestAsync(string requestId)
        {
            return await _context.PublishRequests
                    .Include(r => r.AccessKeys)
                    .Include(r => r.Files)
                    .Include(r => r.SigningKeys)
                    .Include(r => r.RevocationList)
                .Where(r => r.RequestId == requestId)
                .FirstOrDefaultAsync();
        }

        protected async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        protected async Task<string> AcquireLockAsync(string leaseGroup, string requestId, TimeSpan timeSpan)
        {
            var leaseId = await _blobLeaseService.AcquireLeaseAsync(leaseGroup, requestId, timeSpan);

            return leaseId ?? throw new Exception("Lease could not be acquired");
        }

        protected async Task ReleaseLockAsync()
        {
            await _blobLeaseService.ReleaseAsync();
        }
    }

}
