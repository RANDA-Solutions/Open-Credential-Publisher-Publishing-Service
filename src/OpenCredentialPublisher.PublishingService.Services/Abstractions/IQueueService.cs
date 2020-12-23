using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, string message, TimeSpan? visibilityTimeout = null);
    }

}
