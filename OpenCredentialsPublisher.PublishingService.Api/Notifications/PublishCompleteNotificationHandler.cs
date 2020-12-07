using MediatR;
using Newtonsoft.Json;
using OpenCredentialsPublisher.PublishingService.Data;
using OpenCredentialsPublisher.PublishingService.Services;
using OpenCredentialsPublisher.PublishingService.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public class PublishProcessRequestHandler : INotificationHandler<PublishProcessRequestCommand>
    {
        private readonly IQueueService _queueService;

        public PublishProcessRequestHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(PublishProcessRequestCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(PublishQueues.PublishRequest, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(30));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishRequest, JsonConvert.SerializeObject(notification));
#endif
        }
    }

}
