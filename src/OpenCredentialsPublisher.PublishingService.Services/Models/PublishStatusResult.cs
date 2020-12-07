namespace OpenCredentialsPublisher.PublishingService.Services
{
    public class PublishStatusResult
    {
        public string Status { get; set; }
        public string AccessKey { get; set; }

        public ClrPublishQrCode QrCode { get; set; }
    }

    

}
