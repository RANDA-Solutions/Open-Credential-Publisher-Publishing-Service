using MediatR;
using Newtonsoft.Json;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class PublishSignClrNotificationHandler : INotificationHandler<PublishSignClrCommand>
    {
        private readonly IQueueService _queueService;

        public PublishSignClrNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(PublishSignClrCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(PublishQueues.PublishSignClr, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(30));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishSignClr, JsonConvert.SerializeObject(notification));
#endif
        }

    }

}
