using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class IdentifierEntry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("identifierType", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("identifierType")]
        public string IdentifierType { get; set; }
    }
}
