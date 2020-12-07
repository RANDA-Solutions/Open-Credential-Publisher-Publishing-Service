using Newtonsoft.Json;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
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
    }

}
