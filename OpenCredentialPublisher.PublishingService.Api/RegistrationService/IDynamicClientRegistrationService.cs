using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public interface IDynamicClientRegistrationService
    {
        Task<OcpDynamicClientRegistrationResult> RegisterClientAsync(OcpDynamicClientRegistrationRequest model);
    }

}
