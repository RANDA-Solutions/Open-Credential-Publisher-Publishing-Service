using OpenCredentialPublisher.Credentials.Clrs.Clr;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{
    public interface IPublishService
    {
        Task<string> ProcessRequestAsync(string id, ClrDType clr, string clientId);
        Task<PublishStatusResult> GetAsync(string requestId, string clientId, string scope, string endpoint, string method);

        Task RevokeAsync(string requestId, string clientId);

        Task<string> GetCredentialsAsync(string requestId);
    }

    

}
