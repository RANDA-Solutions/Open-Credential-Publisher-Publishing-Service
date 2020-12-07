using Newtonsoft.Json;
using System;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class EndorsementDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("claim", Required = Required.Always)]
        public EndorsementClaimDType Claim { get; set; }
        [JsonProperty("issuedOn", Required = Required.Always)]
        public DateTime IssuedOn { get; set; }
        [JsonProperty("issuer", Required = Required.Always)]
        public EndorsementProfileDType Issuer { get; set; }
        [JsonProperty("revocationReason", NullValueHandling = NullValueHandling.Ignore)]
        public string RevocationReason { get; set; }
        [JsonProperty("revoked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Revoked { get; set; }
        [JsonProperty("verification", Required = Required.Always)]
        public VerificationDType Verification { get; set; }
    }

}
