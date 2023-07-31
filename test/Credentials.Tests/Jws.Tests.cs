//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.KeyStorage;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Tests
{
    public class Jws
    {

        private IKeyStore _keyStorage;

        [SetUp]
        public void Setup()
        {
            _keyStorage = new FileStorage();
        }

        [Test]
        public void ConstructAndValidate()
        {
            IdentityModelEventSource.ShowPII = true;
            const string algorithm = "RS512";
            string keyId = Guid.NewGuid().ToString();

            var contents = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { "prop1", "this thing" },
                { "prop2", "this thing2" }
            });
            using var rsaCrypto = new RSACryptoServiceProvider();
            var signingKey = new RsaSecurityKey(rsaCrypto.ExportParameters(true));
            signingKey.KeyId = keyId;

            var publicKey = _keyStorage.GetPublicKeyAsync(new OcpSigningCredentials()
            {
                Algorithm = algorithm,
                KeyId = keyId,
                Parameters = rsaCrypto.ExportParameters(true)
            }).Result;

            var header = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    { JwtHeaderParameterNames.Alg, algorithm },
                    { JwtHeaderParameterNames.Kid, signingKey.KeyId },
                    { JwtHeaderParameterNames.Typ, "JWT" }
                }));

            var encodedContents = Base64UrlEncoder.Encode(contents);
            var byteData = Encoding.UTF8.GetBytes($"{header}.{encodedContents}");

            using var crypto = new SHA512CryptoServiceProvider();
            var digest = crypto.ComputeHash(byteData);
            var signedBytes = rsaCrypto.SignHash(digest, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            var signature = Base64UrlEncoder.Encode(signedBytes);
            var jwsString = $"{header}.{encodedContents}.{signature}";

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwsString);
            Assert.IsNotNull(token);


        }
    }
}
