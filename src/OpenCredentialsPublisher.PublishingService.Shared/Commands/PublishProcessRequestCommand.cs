using MediatR;

namespace OpenCredentialsPublisher.PublishingService.Shared
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
