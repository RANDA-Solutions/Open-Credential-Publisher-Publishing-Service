using MediatR;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public class PublishSignClrCommand : ICommand, INotification
    {
        public string RequestId { get; }

        public PublishSignClrCommand(string requestId)
        {
            RequestId = requestId;
        }
    }

}
