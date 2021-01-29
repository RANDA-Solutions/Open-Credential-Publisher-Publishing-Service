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
    public class PublishPackageClrNotificationHandler : INotificationHandler<PublishPackageClrCommand>
    {
        private readonly IQueueService _queueService;

        public PublishPackageClrNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(PublishPackageClrCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(PublishQueues.PublishPackageClr, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(Constants.DebugDelay));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishPackageClr, JsonConvert.SerializeObject(notification));
#endif
        }
    }

}
