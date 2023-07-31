using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class CryptographicKeyDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("owner", Required = Required.Always)]
        public string Owner { get; set; }
        [JsonProperty("publicKeyPem", Required = Required.Always)]
        public string PublicKeyPem { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

    }

}
