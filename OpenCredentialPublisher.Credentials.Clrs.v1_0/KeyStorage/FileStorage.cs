using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Keys;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Cryptography;
using PemUtils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.KeyStorage
{
    /// <summary>
    /// Sample implementation based upon IMS Global's CLR Reference Project
    /// https://github.com/IMSGlobal/CLR-reference-implementation/blob/develop/ClrProvider/src/Crypto.cs
    /// </summary>
    public class FileStorage : IKeyStore
    {
        private const string KeyFileName = "keys.rsa";

        private RsaKeySet _keySet = null;

        public async Task<RsaKey> GetKeyAsync(string keyId)
        {
            if (keyId == null)
                return null;

            RsaKeySet rsaKeySet = await GetKeySetAsync();

            return _keySet.Keys.SingleOrDefault(x => x.KeyId == keyId);
        }

        public async Task<RsaKey> GetKeyAsync(string keyId = null, string issuerId = null)
        {
            if (keyId == null && issuerId == null)
                return null;

            RsaKeySet rsaKeySet = await GetKeySetAsync();

            return issuerId == null
                ? _keySet.Keys.SingleOrDefault(x => x.KeyId == keyId)
                : _keySet.Keys.SingleOrDefault(x => x.IssuerId == issuerId);
        }

        public async Task<string> GetPublicKeyAsync(string keyId = null, string issuerId = null)
        {
            throw new NotImplementedException();
        }

        //private RsaKeySet GetKeySet()
        //{
        //    if (_keySet == null)
        //    {
        //        var filename = Path.Combine(Directory.GetCurrentDirectory(), KeyFileName);
        //        if (File.Exists(filename))
        //        {
        //            var keysFile = File.ReadAllText(filename);
        //            _keySet = JsonConvert.DeserializeObject<RsaKeySet>(keysFile, new JsonSerializerSettings
        //            {
        //                ContractResolver = new RsaKeyContractResolver()
        //            });
        //        }
        //    }
        //    return _keySet;
        //}


        public async Task<string> CreateKeyAsync(string keyId, string issuerId = null)
        {
            var key = CreateRsaSecurityKey();

            var parameters = key.Rsa?.ExportParameters(includePrivateParameters: true) ?? key.Parameters;

            var rsaKey = new RsaKey
            {
                Parameters = parameters,
                KeyId = key.KeyId,
                IssuerId = issuerId
            };

            _keySet.Keys.Add(rsaKey);
            await UpdateKeySetAsync();

            // return rsaKey;
            return keyId;
        }

        private async Task<RsaKeySet> GetKeySetAsync()
        {
            if (_keySet == null)
            {
                var filename = Path.Combine(Directory.GetCurrentDirectory(), KeyFileName);
                if (File.Exists(filename))
                {
                    var keysFile = await File.ReadAllTextAsync(filename);
                    _keySet = JsonConvert.DeserializeObject<RsaKeySet>(keysFile, new JsonSerializerSettings
                    {
                        ContractResolver = new RsaKeyContractResolver()
                    });
                }
                else
                {
                    _keySet = new RsaKeySet { Keys = new List<RsaKey>() };
                }
            }
            return _keySet;
        }


        private static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsaBlob = CryptoMethods.GenerateRsaKey();
            var parameters = CryptoMethods.FromCspBlobToRSAParameters(rsaBlob);
            var key = new RsaSecurityKey(parameters) { KeyId = CryptoRandom.CreateUniqueId(16) };
            return key;
        }

        private async Task UpdateKeySetAsync()
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), KeyFileName);
            await File.WriteAllTextAsync(filename, JsonConvert.SerializeObject(_keySet, new JsonSerializerSettings
            {
                ContractResolver = new RsaKeyContractResolver()
            }));
        }

        public async Task<string> SignAsync(OcpSigningCredentials credentials, string contents, string algorithm = "RS512")
        {
            await Task.Delay(0);

            var securityKey = new RsaSecurityKey(credentials.Parameters)
            {
                KeyId = credentials.KeyId
            };

            var signingCredentials = new SigningCredentials(securityKey, credentials.Algorithm);

            var payload = JwtPayload.Deserialize(contents);

            var token = new JwtSecurityToken(new JwtHeader(signingCredentials), payload);

            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        public async Task<string> GetPublicKeyAsync(OcpSigningCredentials credentials)
        {
            var securityKey = new RsaSecurityKey(credentials.Parameters)
            {
                KeyId = credentials.KeyId
            };

            var signingCredentials = new SigningCredentials(securityKey, credentials.Algorithm);


            var key = (RsaSecurityKey)signingCredentials.Key;

            using var stream = new MemoryStream();
            using var writer = new PemWriter(stream);
            writer.WritePublicKey(key.Parameters);
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task<OcpSigningCredentials> GetSigningCredentialsAsync(string issuerId = null, string keyId = null, bool createIfNotExists = true)
        {
            var key = await GetKeyAsync(keyId, issuerId);
            if (key == null && createIfNotExists)
            {
                var keyString = await CreateKeyAsync(keyId, issuerId);
                key = await GetKeyAsync(keyId, issuerId);
            }

            return new OcpSigningCredentials() { Algorithm = SecurityAlgorithms.RsaSha512, KeyId = keyId, IssuerId = issuerId, Parameters = key.Parameters };
        }

        public Task DeleteKeyAsync(string keyName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignProofAsync(OcpSigningCredentials signingCredentials, string contents, string algorithm = "RS512")
        {
            throw new NotImplementedException();
        }
    }




}
