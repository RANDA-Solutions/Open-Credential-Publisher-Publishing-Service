using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class IdentityDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }
        [JsonProperty("identity", Required = Required.Always)]
        public string Identity { get; set; }
        [JsonProperty("hashed", Required = Required.Default)]
        public bool Hashed { get; set; }
        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore)]
        public string Salt { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
