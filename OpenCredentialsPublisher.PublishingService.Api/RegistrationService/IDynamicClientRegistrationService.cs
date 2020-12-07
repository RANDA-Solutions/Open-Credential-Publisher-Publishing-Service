using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public interface IDynamicClientRegistrationService
    {
        Task<OcpDynamicClientRegistrationResult> RegisterClientAsync(OcpDynamicClientRegistrationRequest model);
    }

}
