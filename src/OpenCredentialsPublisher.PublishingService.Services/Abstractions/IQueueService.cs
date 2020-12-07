using System;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, string message, TimeSpan? visibilityTimeout = null);
    }

}
