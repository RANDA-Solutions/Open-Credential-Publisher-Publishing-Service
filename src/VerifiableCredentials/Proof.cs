using Newtonsoft.Json;
using System;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{
    public class Proof
    {
        [JsonProperty("type", Order = 1)]
        public string Type { get; set; }

        [JsonProperty("created", Order = 2)]
        [JsonConverter(typeof(DateConverter<DateTime>), "o")]
        public DateTime Created { get; set; }

        [JsonProperty("proofPurpose", Order = 3)]
        public string ProofPurpose { get; set; }

        [JsonProperty("verificationMethod", Order = 4)]
        public string VerificationMethod { get; set; }

        [JsonProperty("signature", Order = 7)]
        public string Signature { get; set; }

        [JsonProperty("challenge", Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        public string Challenge { get; set; }

        [JsonProperty("domain", Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }
    }
}
