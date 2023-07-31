using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Cryptography;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{

    public class VerifiableCredential
    {
        [JsonProperty("@context", Order = 1)]
        public List<String> Contexts { get; set; }

        [JsonProperty("id", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public String Id { get; set; }

        [JsonProperty("type", Order = 3)]
        public List<String> Types { get; set; }

        [JsonProperty("issuer", Order = 4)]
        public String Issuer { get; set; }

        [JsonProperty("issuanceDate", Order = 5)]
        [JsonConverter(typeof(DateConverter<DateTime>), "o")]
        public DateTime IssuanceDate { get; set; }

        [JsonProperty("credentialSubject", Order = 6)]
        [JsonConverter(typeof(SingleOrArrayConverter<ICredentialSubject>))]
        public List<ICredentialSubject> CredentialSubjects { get; set; }

        [JsonProperty("credentialStatus", Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public CredentialStatus CredentialStatus { get; set; }

        [JsonProperty("proof", Order = 8, NullValueHandling = NullValueHandling.Ignore)]
        public virtual Proof Proof { get; set; }

        public virtual string Sign(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, String challenge = default)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.None);
            json += challenge;

            var signature = CryptoMethods.SignString(keyAlgorithm, keyBytes, json);
            return signature;
        }

        public virtual async Task CreateProof(OcpSigningCredentials credentials, IKeyStore keyStore, ProofPurposeEnum proofPurpose, Uri verificationMethod, String challenge)
        {
            var proof = new Proof()
            {
                Type = credentials.Algorithm == SecurityAlgorithms.RsaSha512 ? ProofTypeEnum.RsaSignature2018.ToString() : ProofTypeEnum.Ed25519Signature2018.ToString(),
                Created = DateTime.UtcNow,
                Challenge = challenge,
                ProofPurpose = proofPurpose.ToString(),
                VerificationMethod = verificationMethod.ToString()
            };

            var json = JsonConvert.SerializeObject(this, Formatting.None);
            json += challenge;

            proof.Signature = await keyStore.SignProofAsync(credentials, json);

            Proof = proof;
        }

        public virtual void CreateProof(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, ProofPurposeEnum proofPurpose, Uri verificationMethod, String challenge)
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

        public virtual Boolean VerifyProof(KeyAlgorithmEnum keyAlgorithm, byte[] publicKeyBytes)
        {
            var proof = Proof;
            Proof = null;

            var json = JsonConvert.SerializeObject(this, Formatting.None);
            json += proof.Challenge;

            Proof = proof;

            return CryptoMethods.VerifySignature(keyAlgorithm, publicKeyBytes, proof.Signature, json);
        }

        public void SetIssuer(Uri uri)
        {
            Issuer = uri.ToString();
        }

        public void SetIssuer(String did, String name)
        {
            var issuer = new Issuer
            {
                Id = did,
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }

        public void SetIssuer(Uri uri, String name)
        {
            var issuer = new Issuer
            {
                Id = uri.ToString(),
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }
         
    }
}
