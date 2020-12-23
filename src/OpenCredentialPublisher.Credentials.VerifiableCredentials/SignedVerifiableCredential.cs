using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Cryptography;
using System;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{
    public class SignedVerifiableCredential: VerifiableCredential
    {
        [JsonProperty("proof", Order = 8, NullValueHandling = NullValueHandling.Ignore)]
        public override Proof Proof { get; set; }

        public override string Sign(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, String challenge = default)
        {
            var json = Serialize((VerifiableCredential)this, false);
            json += challenge;

            var signature = CryptoMethods.SignString(keyAlgorithm, keyBytes, json);
            return signature;
        }

        public override void CreateProof(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, ProofPurposeEnum proofPurpose, Uri verificationMethod, String challenge)
        {
            var proof = new Proof()
            {
                Created = DateTime.UtcNow,
                Challenge = challenge,
                ProofPurpose = proofPurpose.ToString(),
                VerificationMethod = verificationMethod.ToString()
            };

            proof.Type = keyAlgorithm switch
            {
                KeyAlgorithmEnum.Ed25519 => ProofTypeEnum.Ed25519Signature2018.ToString(),
                _ => ProofTypeEnum.RsaSignature2018.ToString()
            };

            proof.Signature = Sign(keyAlgorithm, keyBytes, challenge);

            Proof = proof;
        }

        public override Boolean VerifyProof(KeyAlgorithmEnum keyAlgorithm, byte[] publicKeyBytes)
        {
            var proof = Proof;
            Proof = null;

            var json = Serialize((VerifiableCredential)this, false);
            json += proof.Challenge;

            Proof = proof;
            
            return CryptoMethods.VerifySignature(keyAlgorithm, publicKeyBytes, proof.Signature, json);
        }

        public static String Serialize(Object data, Boolean humanReadable = true)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = VerifiableCredentialsBinder.GetBinder()
            };

            String json;
            if (humanReadable)
            {
                json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
                var testExpression = "^ +\"\\$type\": \"\"," + Environment.NewLine;
                var regex = new Regex(testExpression, RegexOptions.Multiline);
                json = regex.Replace(json, String.Empty);
            }
            else
            {
                json = JsonConvert.SerializeObject(data, Formatting.None, settings);
                var testExpression = "\"\\$type\":\"\",";
                var regex = new Regex(testExpression);
                json = regex.Replace(json, String.Empty);
            }
            return json;
        }

        public static T Deserialize<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = VerifiableCredentialsBinder.GetBinder(),
                DateParseHandling = DateParseHandling.None
            });
        }
    }
}
