namespace OpenCredentialPublisher.PublishingService.Api
{
    public class ConnectWalletResult
    {
        public string ConnectionRequestId { get; set; }
        public bool Success { get; set; }
        public string[] ErrorMessages { get; set; }
    }
}
