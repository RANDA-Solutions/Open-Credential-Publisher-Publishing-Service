using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class EndorsementProfileDType : SharedProfileDType
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public AddressDType Address { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("sourcedId", NullValueHandling = NullValueHandling.Ignore)]
        public string SourcedId { get; set; }
        [JsonProperty("studentId", NullValueHandling = NullValueHandling.Ignore)]
        public string StudentId { get; set; }
        [JsonProperty("telephone", NullValueHandling = NullValueHandling.Ignore)]
        public string Telephone { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("verification", NullValueHandling = NullValueHandling.Ignore)]
        public VerificationDType Verification { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
