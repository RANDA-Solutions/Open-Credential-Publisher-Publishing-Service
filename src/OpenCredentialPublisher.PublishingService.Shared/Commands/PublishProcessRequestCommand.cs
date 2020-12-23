using MediatR;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public class PublishProcessRequestCommand : ICommand, INotification
    {
        public string RequestId { get; }

        public PublishProcessRequestCommand(string requestId)
        {
            RequestId = requestId;
        }
    }

}
