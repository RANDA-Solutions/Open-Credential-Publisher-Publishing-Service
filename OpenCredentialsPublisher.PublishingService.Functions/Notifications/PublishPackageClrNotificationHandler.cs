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
            await _queueService.SendMessageAsync(PublishQueues.PublishPackageClr, JsonConvert.SerializeObject(notification), TimeSpan.FromSeconds(30));
#else
            await _queueService.SendMessageAsync(PublishQueues.PublishPackageClr, JsonConvert.SerializeObject(notification));
#endif
        }
    }

}
