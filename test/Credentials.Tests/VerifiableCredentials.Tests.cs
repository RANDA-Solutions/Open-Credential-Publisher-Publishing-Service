using OpenCredentialsPublisher.Credentials.Cryptography;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialsPublisher.Credentials.VerifiableCredentials;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OpenCredentialsPublisher.Credentials.Clrs.Interfaces;
using OpenCredentialsPublisher.Credentials.Clrs.KeyStorage;
using OpenCredentialsPublisher.Credentials.Clrs.Utilities;
using OpenCredentialsPublisher.Credentials.Clrs.Clr;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Tests
{
    public class VerifiableCredentialsTests
    {
        private string ndClrTranscriptJson = "";
        private IKeyStore _keyStorage;

        [SetUp]
        public void Setup()
        {
            using var stream = new StreamReader(typeof(VerifiableCredentialsTests).Assembly.GetManifestResourceStream($"{typeof(VerifiableCredentialsTests).Namespace}.Files.nd-transcript-with-pdf.json"));
            ndClrTranscriptJson = stream.ReadToEnd();
            _keyStorage = new FileStorage();
        }

        public void DeserializeEtsSubject()
        {
            var credential = JsonConvert.DeserializeObject<VerifiableCredential>(ndClrTranscriptJson);
            var credString = JsonConvert.SerializeObject(credential);
            credential = JsonConvert.DeserializeObject<VerifiableCredential>(credString);
            //foreach (var subject in credential.CredentialSubjects)
            //{
            //    Assert.IsTrue(subject is EtsSubject);
            //}
        }

        public void VerifyProof()
        {
            //var signedCredential = SignedVerifiableCredential.Deserialize<SignedVerifiableCredential>(etsCredentialJson);

            //using var client = new HttpClient();

            //var task = Task.Run(() =>
            //{
            //    var task = client.GetStringAsync(signedCredential.Proof.VerificationMethod);
            //    var result = task.Result;
            //    //var getKeyResponse = JsonConvert.DeserializeObject<GetKeyResponse>(result);
            //    // return getKeyResponse;
            //});
            //var key = task.Result;
            //Assert.IsFalse(key.Error);

            //Assert.IsTrue(signedCredential.VerifyProof(key.KeyType, key.PublicKey));
        }

        [Test]
        public void Deserialize()
        {
            using var stream = new StreamReader(typeof(VerifiableCredentialsTests).Assembly.GetManifestResourceStream($"{typeof(VerifiableCredentialsTests).Namespace}.Files.signedVC-Clr.json"));
            var vcJson = stream.ReadToEnd();

            var signedVC = JsonConvert.DeserializeObject<VerifiableCredential>(vcJson);
            Assert.IsNotNull(signedVC);
        }

        [Test]
        public void SignAndVerify()
        {
            var algorithm = KeyAlgorithmEnum.RSA;
            var keys = CryptoMethods.GenerateKey(algorithm);
            
            var clr = JsonConvert.DeserializeObject<ClrSubject>(ndClrTranscriptJson);
            var signingUtility = new SigningUtility(_keyStorage);
            var baseUri = new System.Uri("https://localhost/api");

            var clrSet = new ClrSetSubject();
            clrSet.Id = clr.Id;
            clrSet.Clrs ??= new List<ClrDType>();
            clrSet.Clrs.Add(clr);
            clrSet.SignedClrs ??= new List<string>();

            var keyId = Guid.NewGuid().ToString();
            var issuerId = Guid.NewGuid().ToString();

            var signedClr = signingUtility.Sign(clr, baseUri, true, _keyStorage.GetSigningCredentialsAsync(issuerId, keyId, true).Result);
            clrSet.SignedClrs.Add(signedClr);

            var verifiableCredential = new VerifiableCredential
            {
                Contexts = new List<string>(new[] { "https://www.w3.org/2018/credentials/v1", "https://contexts.ward.guru/clr_v1p0.jsonld" }),
                Types = new List<string>(new[] { nameof(VerifiableCredential) }),
                Id = $"http://vc.example.com/credentials/{Guid.NewGuid()}",
                Issuer = $"http://vc.example.com/issuers/{Guid.NewGuid()}",
                IssuanceDate = DateTime.UtcNow,
                CredentialSubjects = new List<ICredentialSubject>(new[] { clrSet }),

            };

            var challenge = Guid.NewGuid().ToString();
            verifiableCredential.Proof = null;
            verifiableCredential.CreateProof(algorithm, keys, ProofPurposeEnum.assertionMethod, new Uri($"{baseUri}/keys/{Guid.NewGuid()}"), challenge);
            Assert.IsTrue(verifiableCredential.VerifyProof(algorithm, CryptoMethods.GetPublicKey(algorithm, keys)));
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "signedVerifiableCredential-Clr.json"), JsonConvert.SerializeObject(verifiableCredential, Formatting.Indented));
        }
    }
}