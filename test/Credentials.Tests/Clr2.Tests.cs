using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.KeyStorage;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Drawing;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Credentials.Tests
{
    public class Clr2Tests
    {
        private string clrTestJson = "";
        private string vcClr2 = "";

        [SetUp]
        public void Setup()
        {
            using var stream = new StreamReader(typeof(Clr2Tests).Assembly.GetManifestResourceStream($"{typeof(Clr2Tests).Namespace}.Files.clr2Test.json"));
            clrTestJson = stream.ReadToEnd();

            using var stream2 = new StreamReader(typeof(Clr2Tests).Assembly.GetManifestResourceStream($"{typeof(Clr2Tests).Namespace}.Files.clr2VCTest.json"));
            vcClr2 = stream2.ReadToEnd();
        }

        [Test]
        public async Task ConvertClr()
        {
            var testClr = JsonConvert.DeserializeObject<ClrDType>(clrTestJson);
            Assert.IsNotNull(testClr);
            var transformService = new Clr1_0ToClr2_0Service();
            var requestId = Guid.NewGuid().ToString();
            var clientId = Guid.NewGuid().ToString();
            var publicId = Guid.NewGuid().ToString();
            var appBaseUri = "https://localhost";
            var clr2 = await transformService.Transform(appBaseUri, new PublishRequest { RequestId = requestId, ClientId = clientId, RevocationListId = 1 }, testClr);
            Assert.IsNotNull(clr2);
            var json = JsonConvert.SerializeObject(clr2, Formatting.Indented);
            System.IO.File.WriteAllText($"c:\\temp\\clr2\\{DateTime.Now:yyyy-MM-ddHHmmss}.json", json);

            var proofService = new ProofService();
            var result = await proofService.VerifyProof(json);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task VerifyClrProof()
        {
            var proofService = new ProofService();
            var result = await proofService.VerifyProof(vcClr2);
            Assert.IsTrue(result);
        }

       
    }
}