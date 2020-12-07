using MediatR;
using Newtonsoft.Json;
using OpenCredentialsPublisher.PublishingService.Data;
using OpenCredentialsPublisher.PublishingService.Services;
using OpenCredentialsPublisher.PublishingService.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Functions
{
    public class PublishCompleteNotificationHandler : INotificationHandler<PublishNotifyCommand>
    {
        private readonly IQueueService _queueService;

        public PublishCompleteNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(PublishNotifyCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(PublishQueues.PublishNotify, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(30));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishNotify, JsonConvert.SerializeObject(notification));
#endif
        }
    }

}
