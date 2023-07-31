using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{
    public class VerifiablePresentation
    {
        [JsonProperty("@context", Order = 1)]
        public List<String> Contexts { get; set; }

        [JsonProperty("type", Order = 2)]
        public String Type { get; set; } = "VerifiablePresentation";

        [JsonProperty("verifiableCredential", Order = 3)]
        public List<VerifiableCredential> VerifiableCredential { get; set; }

        [JsonProperty("proof", Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(SingleOrArrayConverter<Proof>))]
        public List<Proof> Proofs { get; set; }
    }
}
