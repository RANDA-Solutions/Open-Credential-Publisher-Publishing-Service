using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{
    public class RevocationListService:  IRevocationListService
    {
        private readonly string _appBaseUri;
        private readonly OcpDbContext _context;
        public RevocationListService(IConfiguration configuration, OcpDbContext context)
        {
            _appBaseUri = configuration["AppBaseUri"];

            _context = context;
        }

        public async Task<RevocationDocument> GetRevocationDocument(string publicId)
        {
            var document = new RevocationDocument();
            var list = await _context.RevocationLists.FirstOrDefaultAsync(rl => rl.PublicId == publicId);
            if (list != null)
            {
                var requests = await _context.PublishRequests.Where(pr => pr.RevocationListId == list.Id && pr.PublishState == PublishStates.Revoked).ToListAsync();
                document.Revocations = requests.Select(r => new Revocation { Id = UriUtility.Combine(new System.Uri(_appBaseUri), "api", "credentials", r.RequestId), Status = r.RevocationReason ?? nameof(RevocationReasons.RevokedByIssuer) }).ToList();
            }
            return document;
        }
    }
}
