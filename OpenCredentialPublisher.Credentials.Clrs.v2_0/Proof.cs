using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Proof
    {
        [JsonProperty("type", Order = 1), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("created", Order = 2), JsonPropertyName("created")]
        [Newtonsoft.Json.JsonConverter(typeof(Converters.Newtonsoft.DateConverter<DateTime>), "o"), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.DateConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("proofPurpose", Order = 3), JsonPropertyName("proofPurpose")]
        public string ProofPurpose { get; set; }

        [JsonProperty("verificationMethod", Order = 4), JsonPropertyName("verificationMethod")]
        public object VerificationMethod { get; set; }

        [JsonProperty("challenge", Order = 6, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("domain", Order = 5, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonProperty("signature", Order = 7, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonProperty("proofValue", Order = 8, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("proofValue")]
        public string ProofValue { get; set; }

        [JsonProperty("jws", Order = 9, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("jws")]
        public string JWS { get; set; }

        [JsonProperty("cryptosuite", Order = 9, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("cryptosuite")]
        public string CryptoSuite { get; set; }

        [JsonProperty("nonce", Order = 10, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [System.Text.Json.Serialization.JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<String, Object> AdditionalProperties { get; set; }

    }

    public class VerificationMethod
    {
        [JsonProperty("id", Order = 1), JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonProperty("type", Order = 2), JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
