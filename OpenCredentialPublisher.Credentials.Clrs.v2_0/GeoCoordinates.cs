using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class GeoCoordinates
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("longitude")]
        public string Longitude { get; set; }
    }
}
