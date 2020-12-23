namespace OpenCredentialPublisher.PublishingService.Services
{
    public class PublishStatusResult
    {
        public string Status { get; set; }
        public string AccessKey { get; set; }
        public string Url { get; set; }
        public ClrPublishQrCode QrCode { get; set; }
    }

    

}
