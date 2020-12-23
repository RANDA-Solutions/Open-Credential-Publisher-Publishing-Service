namespace OpenCredentialPublisher.PublishingService.Api
{
    public class ClrPublishResult
    {
        public string RequestId { get; set; }
        public bool Error { get; set; }
        public string[] ErrorMessage { get; set; }
    }

}
