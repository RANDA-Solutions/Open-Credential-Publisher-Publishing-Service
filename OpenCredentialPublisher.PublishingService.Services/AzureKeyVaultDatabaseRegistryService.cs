using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Keys;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Cryptography;
using OpenCredentialPublisher.PublishingService.Data;
using PemUtils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{

    public class AzureKeyVaultDatabaseRegistryService : IKeyStore
    {
        private readonly AzureKeyVaultOptions _options;
        private readonly OcpDbContext _dbContext;

        public AzureKeyVaultDatabaseRegistryService(IOptions<AzureKeyVaultOptions> kvOptions, OcpDbContext dbContext)
        {
            _options = kvOptions?.Value ?? throw new ArgumentNullException(nameof(kvOptions));
            _dbContext = dbContext;
        }

        private async Task<KeyBundle> GetKeyBundleAsync(string keyId)
        {
            var kvc = GetKeyVaultClient();

            return await kvc.GetKeyAsync(_options.KeyVaultBaseUri, keyId);
        }

        public async Task<KeyBundle> GetKeyBundleAsync(string keyId = null, string issuerId = null)
        {
            if (keyId == null && issuerId == null)
                return null;

            var dbKey = (issuerId == null)
                ? await _dbContext.SigningKeys.Where(k => k.KeyName == keyId).SingleOrDefaultAsync()
                : await _dbContext.SigningKeys.Where(k => k.KeyName == keyId && k.IssuerId == issuerId).SingleOrDefaultAsync();

            var vaultKeyId = dbKey == null ? keyId : dbKey.KeyName;
            var vaultIssuerId = dbKey == null ? issuerId : dbKey.IssuerId;


            var keyBundle = await GetKeyBundleAsync(vaultKeyId);

            return keyBundle;
        }

        public async Task<RsaKey> GetKeyAsync(string keyId = null, string issuerId = null)
        {
            if (keyId == null && issuerId == null)
                return null;

            var dbKey = (issuerId == null)
                ? await _dbContext.SigningKeys.Where(k => k.KeyName == keyId).SingleOrDefaultAsync()
                : await _dbContext.SigningKeys.Where(k => k.KeyName == keyId && k.IssuerId == issuerId).SingleOrDefaultAsync();

            var vaultKeyId = dbKey == null ? keyId : dbKey.KeyName;
            var vaultIssuerId = dbKey == null ? issuerId : dbKey.IssuerId;

            var keyBundle = await GetKeyBundleAsync(vaultKeyId);


            if (keyBundle == null)
            {
                return null;
            }
            return new RsaKey { IssuerId = vaultIssuerId, KeyId = keyId, Parameters = keyBundle.Key.ToRSAParameters(true) };
        }

        public async Task<string> GetPublicKeyAsync(string keyId = null, string issuerId = null)
        {
            if (keyId == null && issuerId == null)
                return null;

            var dbKey = (issuerId == null)
                ? await _dbContext.SigningKeys.Where(k => k.KeyName == keyId).SingleOrDefaultAsync()
                : await _dbContext.SigningKeys.Where(k => k.KeyName == keyId && k.IssuerId == issuerId).SingleOrDefaultAsync();
            if (dbKey.StoredInKeyVault)
            {
                var vaultKeyId = dbKey == null ? keyId : dbKey.KeyName;
                var vaultIssuerId = dbKey == null ? issuerId : dbKey.IssuerId;

                var keyBundle = await GetKeyBundleAsync(vaultKeyId);


                if (keyBundle == null)
                {
                    return null;
                }
                return await GetPublicKeyAsync(keyBundle);
            }
            return dbKey.PublicKey;
        }

        public async Task<(KeyBundle keyBundle, RSAParameters fullKey)> CreateLocalSigningKeyAsync(string keyName)
        {
            var kvc = GetKeyVaultClient();

            var blob = CryptoMethods.GenerateRsaKey(true);
            var rsaParameters = CryptoMethods.FromCspBlobToRSAParameters(blob, true);

            var jsonWebKey = new Microsoft.Azure.KeyVault.WebKey.JsonWebKey(rsaParameters);
            var keyBundle = await kvc.ImportKeyAsync(_options.KeyVaultBaseUri, keyName, jsonWebKey, false);

            // var keyBundle = await kvc.CreateKeyAsync(_keyvaultBaseUri, keyName, "RSA");

            return (keyBundle, rsaParameters);
        }

        public async Task<KeyBundle> CreateKeyBundleAsync(string keyId, string issuerId)
        {
            var kvc = GetKeyVaultClient();
            return await kvc.CreateKeyAsync(_options.KeyVaultBaseUri, keyId, "RSA");
        }

        public async Task<string> CreateKeyAsync(string keyId, string issuerId = null)
        {
            var keyBundle = await CreateKeyBundleAsync(keyId, issuerId);

            return keyBundle.KeyIdentifier.Identifier;
        }

        public async Task<RsaKey> GetKeyAsync(string keyId)
        {
            return await GetKeyAsync(keyId, null);
        }

        public async Task<String> SignAsync(string keyName, string payload, string algorithm = "RS512")
        {
            var payloadBytes = UTF8Encoding.UTF8.GetBytes(payload);

            var digest = ComputeHash(algorithm, payloadBytes);

            var kvc = GetKeyVaultClient();

            var result = await kvc.SignAsync(keyName, algorithm, digest);
            return Base64UrlEncoder.Encode(result.Result);
        }

        public async Task<SecretBundle> GetSecretAsync(string secretName)
        {
            var kvc = GetKeyVaultClient();

            var secretBundle = await kvc.GetSecretAsync(_options.KeyVaultBaseUri, secretName);
            return secretBundle;
        }

        public async Task<string> SignAsync(OcpSigningCredentials signingCredentials, string contents, string algorithm = "RS512")
        {
            string url = $"{signingCredentials.KeyIdentifier}";

            var header = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    { JwtHeaderParameterNames.Alg, algorithm },
                    { JwtHeaderParameterNames.Kid, signingCredentials.KeyIdentifier },
                    { JwtHeaderParameterNames.Typ, "JWT" }
                }));

            var encodedContents = Base64UrlEncoder.Encode(contents);
            var byteData = Encoding.UTF8.GetBytes($"{header}.{encodedContents}");
            var digest = ComputeHash(algorithm, byteData);

            var kvc = GetKeyVaultClient();

            var signature = await kvc.SignAsync(url, algorithm, digest);

            return $"{header}.{encodedContents}.{Base64UrlEncoder.Encode(signature.Result)}";
        }

        public async Task<string> SignProofAsync(OcpSigningCredentials signingCredentials, string contents, string algorithm = "RS512")
        {
            string url = $"{signingCredentials.KeyIdentifier}";

            var encodedBytes = UTF8Encoding.UTF8.GetBytes(contents);

            var digest = ComputeHash(algorithm, encodedBytes);

            var kvc = GetKeyVaultClient();
            var signature = await kvc.SignAsync(url, algorithm, digest);

            return Base64UrlEncoder.Encode(signature.Result);
        }

        private KeyVaultClient GetKeyVaultClient() => 
            new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                async (string authority, string resource, string scope) =>
                {
                    var authContext = new AuthenticationContext(authority);
                    var credential = new ClientCredential(_options.AzureAppClientId, _options.AzureAppClientSecret);
                    AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential);
                    if (result == null)
                    {
                        throw new InvalidOperationException("Failed to retrieve JWT token");
                    }
                    return result.AccessToken;
                }
            ));

        public async Task<string> GetPublicKeyAsync(OcpSigningCredentials signingCredentials)
        {
            var bundle = await GetKeyBundleAsync(signingCredentials.KeyId);
            return await GetPublicKeyAsync(bundle);
        }
        public async Task<string> GetPublicKeyAsync(KeyBundle bundle)
        {
            using var stream = new MemoryStream();
            using var writer = new PemWriter(stream);
            writer.WritePublicKey(bundle.Key.ToRSA(false));
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task<OcpSigningCredentials> GetSigningCredentialsAsync(string issuerId = null, string keyId = null, bool createIfNotExists = true)
        {
            var bundle = GetKeyBundleAsync(keyId, issuerId);

            if (bundle == null && (!createIfNotExists || (issuerId == null)))
            {
                // Only create a key when signing

                return null;
            }

            if (bundle == null)
            {
                await CreateKeyAsync(keyId, issuerId);
            }

            return new OcpSigningCredentials() { Algorithm = SecurityAlgorithms.RsaSha512, KeyId = keyId, IssuerId = issuerId };
        }

        private byte[] ComputeHash(string algorithm, byte[] bytesData) =>
            algorithm switch
            {
                "RS256" => new SHA256CryptoServiceProvider().ComputeHash(bytesData),
                "RS512" => new SHA512CryptoServiceProvider().ComputeHash(bytesData),
                _ => throw new NotImplementedException($"{algorithm} has not been implemented yet.")
            };

        public async Task DeleteKeyAsync(string keyName)
        {
            try
            {
                var kvc = GetKeyVaultClient();
                await kvc.DeleteKeyAsync(_options.KeyVaultBaseUri, keyName);

                return;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }

}
