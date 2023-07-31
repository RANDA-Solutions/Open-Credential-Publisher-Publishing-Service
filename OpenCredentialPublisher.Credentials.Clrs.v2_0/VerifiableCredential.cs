using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public abstract class VerifiableCredential : IVerifiableCredential
        
    {
        [JsonProperty("@context", Order = 1, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("@context")]
        public string[] Context { get; set; }

        [JsonProperty("type", Order = 2, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("id", Order = 3, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name", Order = 4, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description", Order = 5, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("description")]
        public string Description { get; set; }



        [JsonProperty("image", Order = 6, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonProperty("credentialSubject", Order =7,  NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("credentialSubject")]
        public CredentialSubject CredentialSubject { get; set; }

        [JsonProperty("issuer", Order = 8, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("issuer")]
        public Profile Issuer { get; set; }

        [JsonProperty("issuanceDate", Order = 9, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("issuanceDate")]
        public string IssuanceDate { get; set; }

        [JsonProperty("expirationDate", Order = 10, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("proof", Order = 11, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("proof")]
        public Proof[] Proof { get; set; }

        [JsonProperty("credentialSchema", Order = 12,  NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("credentialSchema")]
        public BasicProperties[] CredentialSchema { get; set; }

        [JsonProperty("credentialStatus", Order = 13, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("credentialStatus")]
        public BasicProperties CredentialStatus { get; set; }

        [JsonProperty("refreshService", Order = 14, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("refreshService")]
        public BasicProperties RefreshService { get; set; }

        [JsonProperty("termsOfUse", Order = 15, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("termsOfUse")]
        public BasicProperties[] TermsOfUse { get; set; }
    }
}
