using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialPublisher.Credentials.Clrs.Clr;
using OpenCredentialPublisher.Credentials.Clrs.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.KeyStorage;
using OpenCredentialPublisher.Credentials.Clrs.Utilities;
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
            using var stream = new StreamReader(typeof(ClrTests).Assembly.GetManifestResourceStream($"{typeof(ClrTests).Namespace}.Files.nd-clr-transcript.json"));
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

            var signedClr = signingUtility.Sign(transcriptClr, baseUri);
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
            var pdfBytes = PdfUtility.AppendQRCodePage(fileBytes, $"https://opencredentialpublisher.org/access/{accessKey}");
            Assert.IsTrue(pdfBytes.Length > fileBytes.Length);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "pdf-with-qr-code.pdf"), pdfBytes);
        }
    }
}