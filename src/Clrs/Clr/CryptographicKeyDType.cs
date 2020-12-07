using Newtonsoft.Json;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
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

    }

}
