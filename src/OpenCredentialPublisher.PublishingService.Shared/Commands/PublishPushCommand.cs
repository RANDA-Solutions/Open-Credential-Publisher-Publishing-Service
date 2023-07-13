using MediatR;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public class PublishPushCommand : ICommand, INotification
    {
        public string RequestId { get; }

        public PublishPushCommand(string requestId)
        {
            RequestId = requestId;
        }
    }

}
