using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.KeyStorage;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Drawing;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.Credentials.Tests
{
    public class ClrTests
    {
        private string ndClrTranscriptJson = "";
        private IKeyStore _keyStorage;

        [SetUp]
        public void Setup()
        {
            using var stream = new StreamReader(typeof(ClrTests).Assembly.GetManifestResourceStream($"{typeof(ClrTests).Namespace}.Files.CLR-SIGNED-ASSERTIONS.json"));
            ndClrTranscriptJson = stream.ReadToEnd();
            _keyStorage = new FileStorage();
        }

        [Test]
        public void DeserializeTranscript()
        {
            var transcriptClr = JsonConvert.DeserializeObject<ClrDType>(ndClrTranscriptJson);
            Assert.IsNotNull(transcriptClr);
            Assert.AreEqual(transcriptClr.Id, "urn:uuid:f193ab9d-b534-5672-80e9-1c039aded859");
        }

        [Test]
        public void SignClr()
        {
            var signedClr = GetSignedClr();
            var clrSet = new ClrSetDType();
            clrSet.SignedClrs ??= new List<string>();
            clrSet.SignedClrs.Add(signedClr);
            Assert.IsTrue(Regex.IsMatch(signedClr, @"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$"));
        }

        [Test]
        public void ClrFromJws()
        {
            var originalClr = JsonConvert.DeserializeObject<ClrDType>(ndClrTranscriptJson);
            var signedClr = GetSignedClr();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(signedClr);
            var payloadString = token.Payload.SerializeToJson();
            var clr = JsonConvert.DeserializeObject<ClrDType>(payloadString);
            Assert.AreEqual(originalClr.Id, clr.Id);
        }

        private string GetSignedClr()
        {
            var transcriptClr = JsonConvert.DeserializeObject<ClrDType>(ndClrTranscriptJson);
            var signingUtility = new SigningUtility(_keyStorage);
            var baseUri = new System.Uri("https://localhost/api");
            var credentials = _keyStorage.GetSigningCredentialsAsync("getsignedclr-test", "test100", true).Result;
            var signedClr = signingUtility.Sign(transcriptClr, baseUri, false, credentials);
            return signedClr;
        }

        [Test]
        public void PdfToDataUrl()
        {
            using var stream = typeof(ClrTests).Assembly.GetManifestResourceStream($"{typeof(ClrTests).Namespace}.Files.SampleTranscript.pdf");
            var fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            var dataUrl = DataUrlUtility.PdfToDataUrl(fileBytes);
            var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(dataUrl);
            Assert.AreEqual(fileBytes, bytes);
            Assert.AreEqual(DataUrlUtility.PdfMimeType, mimeType);
        }

        [Test]
        public void AddQRCodeToPdf()
        {
            using var stream = typeof(ClrTests).Assembly.GetManifestResourceStream($"{typeof(ClrTests).Namespace}.Files.SampleTranscript.pdf");
            var fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            var accessKey = $"{Guid.NewGuid()}";
            var pdfBytes = PdfUtility.AppendQRCodePage(fileBytes, "https://ocp-wallet-qa.azurewebsites.net/connect?Issuer=https%3a%2f%2frandaocpservice-test.azurewebsites.net&Scope=ocp-wallet&Method=POST&Endpoint=ocp_credentials_endpoint&Payload=%7b%22AccessKey%22%3a%22cbf3af1d-ce4e-4534-b7cc-6d2bb9442389%22%7d", PdfUtility.PageOutlineTitle);
            Assert.IsTrue(pdfBytes.Length > fileBytes.Length);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "pdf-with-qr-code.pdf"), pdfBytes);
        }
    }
}