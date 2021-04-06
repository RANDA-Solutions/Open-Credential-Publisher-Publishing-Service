using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class ClrDType
    {
        [JsonProperty("@context")]
        [JsonConverter(typeof(SingleOrArrayConverter<String>))]
        public List<String> Context { get; set; }
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = "Clr";
        [JsonProperty("achievements")]
        public List<AchievementDType> Achievements { get; set; }
        [JsonProperty("assertions")]
        public List<AssertionDType> Assertions { get; set; }
        [JsonProperty("issuedOn", Required = Required.Always)]
        public DateTime IssuedOn { get; set; }
        [JsonProperty("learner", Required = Required.Always)]
        public ProfileDType Learner { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public String Name { get; set; }
        [JsonProperty("partial")]
        public bool Partial { get; set; }
        [JsonProperty("publisher", Required = Required.Always)]
        public ProfileDType Publisher { get; set; }
        [JsonProperty("revocationReason")]
        public string RevocationReason { get; set; }
        [JsonProperty("revoked")]
        public bool Revoked { get; set; }
        [JsonProperty("signedAssertions")]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public List<string> SignedAssertions { get; set; }
        [JsonProperty("signedEndorsements")]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public List<string> SignedEndorsements { get; set; }
        [JsonProperty("verification")]
        public VerificationDType Verification { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
