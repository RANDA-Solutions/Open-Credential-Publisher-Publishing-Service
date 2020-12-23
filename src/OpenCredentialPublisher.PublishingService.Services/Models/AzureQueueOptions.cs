namespace OpenCredentialPublisher.PublishingService.Services
{
    public class AzureQueueOptions
    {
        public const string Section = "AzureQueue";

        public string StorageConnectionString { get; set; }
    }

}
