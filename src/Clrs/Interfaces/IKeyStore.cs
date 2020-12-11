using Microsoft.IdentityModel.Tokens;
using OpenCredentialsPublisher.Credentials.Clrs.Keys;
using OpenCredentialsPublisher.Credentials.Clrs.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.Credentials.Clrs.Interfaces
{

    public interface IKeyStore
    {
        Task<RsaKey> GetKeyAsync(string keyId);
        Task<RsaKey> GetKeyAsync(string keyId = null, string issuerId = null);

        Task<string> CreateKeyAsync(string keyId, string issuerId = null);

        Task DeleteKeyAsync(string keyName);

        Task<OcpSigningCredentials> GetSigningCredentialsAsync(string issuerId = null, string keyId = null, bool createIfNotExists = true);

        Task<string> SignAsync(OcpSigningCredentials signingCredentials, string contents, string algorithm = "RS512");

        Task<string> GetPublicKeyAsync(OcpSigningCredentials signingCredentials);
    }


}
