using System.Security.Cryptography;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities
{
    public class OcpSigningCredentials
    {
        public RSAParameters Parameters { get; set; }
        public string KeyId { get; set; }
        public string Algorithm { get; set; }
        public string IssuerId { get; set; }

        public string KeyIdentifier { get; set; }
    }

}
