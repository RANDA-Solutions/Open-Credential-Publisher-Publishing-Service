

using MediatR;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public class PublishNotifyCommand : ICommand, INotification
    {
        public string RequestId { get; }

        public PublishNotifyCommand(string requestId)
        {
            RequestId = requestId;
        }
    }

}
