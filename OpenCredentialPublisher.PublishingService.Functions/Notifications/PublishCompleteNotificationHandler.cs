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
            await _queueService.SendMessageAsync(PublishQueues.PublishNotify, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(Constants.DebugDelay));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishNotify, JsonConvert.SerializeObject(notification));
#endif
        }
    }

}
