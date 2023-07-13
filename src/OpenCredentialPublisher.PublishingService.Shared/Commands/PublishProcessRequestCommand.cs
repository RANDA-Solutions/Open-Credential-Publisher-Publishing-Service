using MediatR;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public class PublishProcessRequestCommand : ICommand, INotification
    {
        public string RequestId { get; }
        public bool PushPackage { get; }
        public string PushUri { get; }

        public PublishProcessRequestCommand(string requestId, bool pushPackage = false, string pushUri = null)
        {
            RequestId = requestId;
            PushPackage = pushPackage;
            PushUri = pushUri;
        }
    }

}
