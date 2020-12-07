using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class AssertionDType
    {
        /// <summary>
        /// Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("achievement", NullValueHandling = NullValueHandling.Ignore)]
        public AchievementDType Achievement { get; set; }
        [JsonProperty("creditsEarned", NullValueHandling = NullValueHandling.Ignore)]
        public float? CreditsEarned { get; set; }
        [JsonProperty("endDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
        [JsonProperty("endorsements", NullValueHandling = NullValueHandling.Ignore)]
        public List<EndorsementDType> Endorsements { get; set; }
        [JsonProperty("evidence", NullValueHandling = NullValueHandling.Ignore)]
        public List<EvidenceDType> Evidence { get; set; }
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
        [JsonProperty("issuedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? IssuedOn { get; set; }
        [JsonProperty("licenseNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string LicenseNumber { get; set; }
        /// <summary>
        /// Markdown formatted string
        /// </summary>
        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        public string Narrative { get; set; }
        [JsonProperty("recipient", NullValueHandling = NullValueHandling.Ignore)]
        public IdentityDType Recipient { get; set; }
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<ResultDType> Results { get; set; }
        [JsonProperty("revocationReason", NullValueHandling = NullValueHandling.Ignore)]
        public string RevocationReason { get; set; }
        [JsonProperty("revoked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Revoked { get; set; }
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }
        [JsonProperty("signedEndorsements", NullValueHandling = NullValueHandling.Ignore)]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public List<string> SignedEndorsements { get; set; }
        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }
        [JsonProperty("term", NullValueHandling = NullValueHandling.Ignore)]
        public string Term { get; set; }
        [JsonProperty("verification", NullValueHandling = NullValueHandling.Ignore)]
        public VerificationDType Verification { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
