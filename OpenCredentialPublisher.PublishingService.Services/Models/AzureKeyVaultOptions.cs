namespace OpenCredentialPublisher.PublishingService.Services
{
    public class AzureKeyVaultOptions
    {
        public const string Section = "AzureKeyVault";

        // This is the ID which can be found as "Application (client) ID" when selecting the registered app under "Azure Active Directory" -> "App registrations".
        public string AzureAppClientId { get; set; }
        // This is the client secret from the app registration process.
        public string AzureAppClientSecret { get; set; }
        // This is available as "DNS Name" from the overview page of the Key Vault.
        public string KeyVaultBaseUri { get; set; }

        public string CertificateName { get; set; }
        public string AzureTenantId { get; set; }

        public bool UseRoleBasedAccess { get; set; } = false;
    }

}
