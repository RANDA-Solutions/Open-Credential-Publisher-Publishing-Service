using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Criteria
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("narrative")]
        public string Narrative { get; set; }
    }
}
