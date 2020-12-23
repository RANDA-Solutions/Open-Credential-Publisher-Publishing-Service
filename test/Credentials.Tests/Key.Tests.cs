using OpenCredentialPublisher.Credentials.Cryptography;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Tests
{
    public class KeyTests
    {
        [Test]
        public void CSPtoCNG()
        {
            String publicKeyString;
            byte[] keyblob = CryptoMethods.GenerateRsaKey();
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(keyblob);
                byte[] byteArray = new byte[provider.KeySize];
                var publicKeyBytes = new Span<byte>(byteArray);
                provider.TryExportRSAPublicKey(publicKeyBytes, out int bytesWritten);
                publicKeyString = Convert.ToBase64String(publicKeyBytes.Slice(0, bytesWritten));
                keyblob = provider.ExportPkcs8PrivateKey();
            }

            using (var provider = new RSACng()) // CNG calls Windows CNG Libraries and is not for cross-platform use
            {
                provider.ImportPkcs8PrivateKey(keyblob, out int bytesRead);
                var publicKeyBytes = new Span<byte>(new byte[provider.KeySize]);
                Assert.IsTrue(provider.TryExportRSAPublicKey(publicKeyBytes, out int bytesWritten));
                Assert.AreEqual(Convert.FromBase64String(publicKeyString), publicKeyBytes.Slice(0, bytesWritten).ToArray());
            }
        }
    }
}
