using OpenCredentialsPublisher.Credentials.Clrs.Clr;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Services
{
    public interface IPublishService
    {
        Task<string> ProcessRequestAsync(string id, ClrDType clr, string clientId);
        Task<PublishStatusResult> GetAsync(string requestId, string clientId);
    }

    

}
