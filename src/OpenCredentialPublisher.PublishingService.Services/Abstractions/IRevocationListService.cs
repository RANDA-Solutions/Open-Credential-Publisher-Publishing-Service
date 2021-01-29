using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services.Abstractions
{
    public interface IRevocationListService
    {
        Task<RevocationDocument> GetRevocationDocument(string publicId);
    }
}
