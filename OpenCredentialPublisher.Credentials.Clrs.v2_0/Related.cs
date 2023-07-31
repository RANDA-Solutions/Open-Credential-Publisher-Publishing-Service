using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Related
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("@language", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("@language")]
        public string Language { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
