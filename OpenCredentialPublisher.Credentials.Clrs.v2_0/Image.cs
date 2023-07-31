using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Image
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("caption")]
        public string Caption { get; set; }
    }
}
