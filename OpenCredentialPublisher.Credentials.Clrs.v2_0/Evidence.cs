using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Evidence
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("genre")]
        public string Genre { get; set; }

        [JsonProperty("audience", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("audience")]
        public string Audience { get; set; }
    }
}
