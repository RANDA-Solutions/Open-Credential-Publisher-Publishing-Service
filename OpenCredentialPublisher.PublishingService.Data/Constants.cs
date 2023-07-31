namespace OpenCredentialPublisher.PublishingService.Data
{
    public static class DiscoveryDocumentCustomEndpointsConstants
    {
        public const string ConfigEndpoint = "ocp_config_endpoint";
        public const string CredentialsEndpoint = "ocp_credentials_endpoint";
        public const string KeysEndpoint = "ocp_keys_endpoint";
        public const string PublishEndpoint = "ocp_publish_endpoint";
        public const string RegistrationEndpoint = "registration_endpoint";
        public const string RequestsEndpoint = "ocp_requests_endpoint";
        public const string WalletConnectEndpoint = "ocp_walletconnect_endpoint";
    }

    public static class ScopeConstants
    {
        public const string Publisher = "ocp-publisher";
        public const string Wallet = "ocp-wallet";
    }

    public static class PublishServiceConstants
    {
        public const string ClrFileBlobContainerName = "publishclr";
        public const string LeaseBlobContainerName = "blobleases";
    }

    public static class PublishQueues
    {
        public const string PublishRequest = "pub-processrequest";
        public const string PublishPackageClr = "pub-packageclr";
        public const string PublishSignClr = "pub-signclr";
        public const string PublishNotify = "pub-notify";
        public const string PublishPush = "pub-push";
    }

    public static class PublishStates
    {
        public const string Accepted = "Accepted";
        public const string Packaging = "Packaging";
        public const string SignClr = "Signing";
        public const string Complete = "Complete";
        public const string Revoked = "Revoked";
        public const string Pushing = "Pushing";
    }

    public static class Pathways
    {
        public const string Publish1_0 = "1.0";
        public const string Publish2_0 = "2.0";
    }

    public static class CryptoSuites
    {
        public const string Ed25519Signature2020 = "Ed25519Signature2020";
        public const string RsaSignature2018 = "RsaSignature2018";
    }

    public static class Formats
    {
        public const string DateTimeZFormat = "yyyy-MM-dd'T'HH:mm:ssK";
        //public const string DateTimeFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:sszzz";


    }


    public static class PublishProcessingStates
    {
        public const string PublishRequestReady = "PubRequest-Ready";
        public const string PublishRequestProcessing = "PubRequest-Processing";
        public const string PublishRequestFailure = "PubRequest-Failure";
        public const string PublishPackageClrReady = "PubPackageClr-Ready";
        public const string PublishPackageClrProcessing = "PubPackageClr-Processing";
        public const string PublishPackageClrFailure = "PubPackageClr-Failure";
        public const string PublishSignClrReady = "PubSignClr-Ready";
        public const string PublishSignClrProcessing = "PubSignClr-Processing";
        public const string PublishSignClrFailure = "PubSignClr-Failure";
        public const string PublishNotifyReady = "PubNotify-Ready";
        public const string PublishNotifyProcessing = "PubNotify-Processing";
        public const string PublishNotifyFailure = "PubNotify-Failure";
        public const string Complete = "Complete";
        public const string RevokedByClient = "PubRevoke-ByClient";
        public const string RevokedByIssuer = "PubRevoke-ByIssuer";
        public const string PublishPushReady = "PubPush-Ready";
        public const string PublishPushProcessing = "PubPush-Processing";
        public const string PublishPushFailure = "PubPush-Failure";

    }

    public static class ClrFileTypes
    {
        public const string OriginalClr = "OriginalClr";
        public const string QrCodeImprintedPdf = "PdfQrCode";
        public const string VCWrappedClr = "VCWrappedClr";
        public const string SignedClr = "SignedClr";
        public const string QrCodeImprintedClr = "ModifiedClr+PdfQrCode";
    }


}
