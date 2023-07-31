using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Credentials.Clrs.v2_0;
using OpenCredentialPublisher.Credentials.Cryptography;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
//using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.JsonLD;
using PemUtils;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace OpenCredentialPublisher.PublishingService.Services
{
    public class ProofService
    {

        private readonly Regex _keyDid;
        private readonly Regex _isDid;
        private readonly Regex _isUrl;
        private readonly Regex _jwsRegex;


        public ProofService()
        {
            _jwsRegex = new Regex("(?<header>[a-zA-Z0-9+_]+)?\\.(?<body>[a-zA-Z0-9+_]+)?\\.(?<signature>[a-zA-Z0-9+-_]+)");
            _keyDid = new Regex("did:key:(?<key>[a-km-zA-HJ-NP-Z1-9]+)");
            _isDid = new Regex("^did:");
            _isUrl = new Regex("^http[s]?:");
        }

        public async Task<Credentials.Clrs.v2_0.Proof> CreateProof(string originalJson = null, string verificationMethod = null, byte[] privateKeyBytes = null, byte[] publicKeyBytes = null)
        {
            JObject document;
            using (var reader = new JsonTextReader(new StringReader(originalJson)) { DateParseHandling = DateParseHandling.None })
                document = JObject.Load(reader);

            if (privateKeyBytes == null)
            {
                (var publicKey, var privateKey) = CryptoMethods.GenerateEd25519Keys();

                privateKeyBytes = privateKey;
                publicKeyBytes = publicKey;
            }

            if (verificationMethod == null)
            {
                var did = CryptoMethods.GetDIDKeyEd25519Public(publicKeyBytes);
                verificationMethod = $"{did}#{did.Substring(did.LastIndexOf(":")).Trim(':')}";
            }

            var proof = new Credentials.Clrs.v2_0.Proof();
            proof.VerificationMethod = verificationMethod;
            proof.Created = DateTime.UtcNow;
            proof.Nonce = WebEncoders.Base64UrlEncode(UTF8Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            proof.Type = "Ed25519Signature2020";
            proof.ProofPurpose = "assertionMethod";

            var proofString = System.Text.Json.JsonSerializer.Serialize(proof, new JsonSerializerOptions {  DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
            JToken proofToken;
            using (var reader = new JsonTextReader(new StringReader(proofString)) { DateParseHandling = DateParseHandling.None })
                proofToken = JToken.Load(reader);

            var jsonldSignature = new JsonLinkedDataSignature();
            var verifydata = jsonldSignature.CreateVerifyData(document, proofToken);
            var signature = CryptoMethods.SignBytes(KeyAlgorithmEnum.Ed25519, privateKeyBytes, verifydata);

            proof.ProofValue = signature;
            return proof;
        }

        public async Task<string> CanonicalizeCredential(string json)
        {
            JObject document;
            using (var reader = new JsonTextReader(new StringReader(json)) { DateParseHandling = DateParseHandling.None })
                document = JObject.Load(reader);

            var proof = document["proof"].DeepClone() as JArray;
            document.Remove("proof");
            var jsonDocument = document.ToString();
            var canonicalizedDocument = await JsonLd.Normalization.JsonLdHandler.Canonize(jsonDocument, new JsonLd.Normalization.ExpandOptions { Base = "c14n" });
            StringBuilder documentString = new StringBuilder(canonicalizedDocument);
            using SHA256 sha256 = SHA256.Create();
            byte[] textBytes = Encoding.UTF8.GetBytes(canonicalizedDocument);
            var documentHash = sha256.ComputeHash(textBytes);

            documentString.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<");
            if (proof != null)
            {
                foreach (var item in proof)
                {
                    var proofValue = item["proofValue"];
                    var verificationMethod = item["verificationMethod"];

                    var proofObject = JsonConvert.DeserializeObject<Credentials.Clrs.v2_0.Proof>(item.ToString());
                    var publicKeyBytes = await GetPublicKeyAsync(proofObject);
                    var canonizedProof = await CanonizeProof(document, item);
                    documentString.AppendLine(canonizedProof);
                    documentString.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<");
                    documentString.AppendLine(BitConverter.ToString(publicKeyBytes).Replace("-", "").ToLower());
                    documentString.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<");
                    var proofHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(canonizedProof));

                    byte[] array = new byte[proofHash.Length + documentHash.Length];
                    proofHash.CopyTo(array, 0);
                    documentHash.CopyTo(array, proofHash.Length);
                    documentString.AppendLine(BitConverter.ToString(array).Replace("-", "").ToLower());
                    documentString.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<");
                    var proofValueString = proofValue.ToString();
                    if (proofValueString != null)
                    {
                        byte[] signatureBytes;
                        if (proofValueString.StartsWith('z'))
                        {
                            signatureBytes = CryptoMethods.Base58DecodeString(proofValueString);

                        }
                        else
                        {
                            signatureBytes = WebEncoders.Base64UrlDecode(proofValueString);
                        }
                        documentString.AppendLine(BitConverter.ToString(signatureBytes).Replace("-", "").ToLower());
                        documentString.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<");
                    }
                    
                }
            }

            return documentString.ToString();
        }

        public async Task<string> CanonizeProof(JObject document, JToken proof)
        {
            System.Text.Json.JsonSerializer.Serialize(proof);
            JObject proofDocument = JObject.Parse("{}");
            proofDocument.TryAdd("@context", document["@context"]);
            proofDocument.Merge(proof);
            proofDocument.Remove("jws");
            proofDocument.Remove("signatureValue");
            proofDocument.Remove("proofValue");
            proofDocument.Remove("signature");

            return await JsonLd.Normalization.JsonLdHandler.Canonize(proofDocument.ToString(), new JsonLd.Normalization.ExpandOptions { Base = "c14n" });
        }

        public async Task<Boolean> VerifyProof(string originalJson = null)
        {
            JObject document;
            using (var reader = new JsonTextReader(new StringReader(originalJson)) { DateParseHandling = DateParseHandling.None })
                document = JObject.Load(reader);
            if (!document.ContainsKey("proof"))
            {
                throw new InvalidDataException("Missing proof");
            }
            var proofs = document["proof"];
            document.Remove("proof");
            var verified = false;
            if (proofs.Type == JTokenType.Array)
            {
                foreach (var proofJson in proofs.Children())
                {
                    verified = await ProcessProof(document, proofJson);
                    if (!verified)
                        break;
                }
            }
            else if (proofs.Type == JTokenType.Object)
            {
                verified = await ProcessProof(document, proofs);
            }

            return verified;
        }

        private async Task<bool> ProcessProof(JObject document, JToken proofJson)
        {
            var jsonldSignature = new JsonLinkedDataSignature();
            var proof = System.Text.Json.JsonSerializer.Deserialize<Credentials.Clrs.v2_0.Proof>(proofJson.ToString());

            var algorithm = proof.Type switch
            {
                "Ed25519Signature2018" => KeyAlgorithmEnum.Ed25519,
                "Ed25519Signature2020" => KeyAlgorithmEnum.Ed25519,
                "Ed25519VerificationKey2020" => KeyAlgorithmEnum.Ed25519,
                _ => KeyAlgorithmEnum.RSA
            };
            var verifydata = jsonldSignature.CreateVerifyData(document, proofJson);
            var publicKeyBytes = await GetPublicKeyAsync(proof);

            var signature = GetSignature(proof);
            byte[] signatureBytes;
            if (signature.StartsWith("z"))
            {
                signatureBytes = CryptoMethods.Base58DecodeString(signature);
                
            }
            else
            {
                signatureBytes = WebEncoders.Base64UrlDecode(signature);
            }

            if (!String.IsNullOrEmpty(proof.JWS) && _jwsRegex.IsMatch(proof.JWS))
            {
                var match = _jwsRegex.Match(proof.JWS);
                var header = match.Groups["header"].Value;
                var headerJson = UTF8Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(header));
                var jwtHeader = System.Text.Json.JsonSerializer.Deserialize<JwtHeader>(headerJson);
                var headerBytes = Encoding.UTF8.GetBytes(header + ".");
                byte[] jws;
                if (jwtHeader.B64)
                {
                    var encodedVerifyDataBytes = Encoding.UTF8.GetBytes(WebEncoders.Base64UrlEncode(verifydata));
                    jws = new byte[headerBytes.Length + encodedVerifyDataBytes.Length];
                    headerBytes.CopyTo(jws, 0);
                    encodedVerifyDataBytes.CopyTo(jws, headerBytes.Length);
                }
                else
                {
                    jws = new byte[headerBytes.Length + verifydata.Length];
                    headerBytes.CopyTo(jws, 0);
                    verifydata.CopyTo(jws, headerBytes.Length);
                }

                return CryptoMethods.VerifySignature(algorithm, publicKeyBytes, signatureBytes, jws);
            }
            else
            {
                return CryptoMethods.VerifySignature(algorithm, publicKeyBytes, signatureBytes, verifydata);
            }
        }

        private string GetBody(string json, Credentials.Clrs.v2_0.Proof proof)
        {
            if (!string.IsNullOrEmpty(proof?.JWS))
            {
                var match = _jwsRegex.Match(proof.JWS);

                //if (!match.Success && proof.JWS == "BadProof") throw new FormatException("Bad Proof.JWS.");
                if (!match.Success) throw new FormatException("Proof.JWS no match.");
                var headerJson = string.Empty;
                var header = match.Groups["header"].Value;
                try
                {
                    headerJson = UTF8Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(header));
                }
                catch (FormatException fe)
                {
                    throw;
                }
                var jwtHeader = System.Text.Json.JsonSerializer.Deserialize<JwtHeader>(headerJson);

                if (jwtHeader.B64)
                {
                    return $"{header}.{WebEncoders.Base64UrlEncode(UTF8Encoding.UTF8.GetBytes(json))}";
                }
                else
                {
                    return $"{header}.{json}";
                }
            }
            if (!string.IsNullOrEmpty(proof?.Challenge))
            {
                var builder = new StringBuilder(json);
                builder.Append(proof.Challenge);
                return builder.ToString();
            }
            return json;
        }

        private async Task<byte[]> GetPublicKeyAsync(Credentials.Clrs.v2_0.Proof proof)
        {
            string verificationMethod = string.Empty;

            if (proof.VerificationMethod is VerificationMethod val)
            {
                verificationMethod = val.Id;
            }
            else
            {
                verificationMethod = proof.VerificationMethod.ToString();
            }
            if (_isDid.IsMatch(verificationMethod))
            {
                if (_keyDid.IsMatch(verificationMethod))
                {
                    var match = _keyDid.Match(verificationMethod);
                    var group = match.Groups["key"];
                    var keyValue = group.Value;
                    var bytes = CryptoMethods.Base58DecodeString(keyValue);
                    var length = bytes.Length - 32;
                    if (length > 0)
                    {
                        var multiCodec = bytes.Take(length).ToArray();
                        return bytes.Skip(length).ToArray();
                    }
                    return bytes;
                }
                throw new NotImplementedException($"The did method {proof.VerificationMethod} is not supported");
            }
            if (_isUrl.IsMatch(verificationMethod))
            {
                var verificationUrl = SanitizePath(verificationMethod);
                var publicKey = await GetKeyAsync(verificationUrl);

                if (proof.Type == ProofTypeEnum.RsaSignature2018.ToString())
                {
                    RSAParameters rsaParameters;
                    await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(publicKey)))
                    {
                        using var reader = new PemReader(stream);
                        rsaParameters = reader.ReadRsaKey();
                    }

                    using var crypto = new RSACryptoServiceProvider();
                    crypto.ImportParameters(rsaParameters);
                    return crypto.ExportCspBlob(false);
                }
                if (proof.Type == ProofTypeEnum.Ed25519Signature2018.ToString())
                {
                    return CryptoMethods.Base58DecodeString(publicKey);
                }
            }
            return null;
        }

        public async Task<string> GetKeyAsync(String url)
        {
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            if (response.Content is object)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception("There was an issue processing your connect request.");
        }

        private string SanitizePath(string url)
        {
            var builder = new UriBuilder(url);
            if (builder.Path.StartsWith("//"))
            {
                builder.Path = builder.Path.Trim('/');
            }
            return builder.ToString();
        }

        private string GetSignature(Credentials.Clrs.v2_0.Proof proof)
        {
            if (proof.JWS != null && _jwsRegex.IsMatch(proof.JWS))
            {
                var jwsMatch = _jwsRegex.Match(proof.JWS);
                return jwsMatch.Groups["signature"].Value;
            }
            else if (proof.Signature != null)
            {
                return proof.Signature;
            }
            else
            {
                return proof.ProofValue;
            }
        }
    }
}
