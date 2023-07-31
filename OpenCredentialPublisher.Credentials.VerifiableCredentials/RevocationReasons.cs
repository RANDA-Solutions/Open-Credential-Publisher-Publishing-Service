using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{
    public static class RevocationReasons
    {
        public const string RevokedByIssuer = "Credential revoked by issuer";
        public const string RevokedByHolder = "Credential revoked at request of credential holder";
        public const string SupersededByIssuer = "Issuer has issued a new credential that supersedes this credential";
    }
}
