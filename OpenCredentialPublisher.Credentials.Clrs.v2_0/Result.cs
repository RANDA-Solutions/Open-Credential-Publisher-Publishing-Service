using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Result
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("achievedLevel", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("achievedLevel")]
        public string AchievedLevel { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("alignment")]
        public Alignment[] Alignment { get; set; }

        [JsonProperty("resultDescription", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("resultDescription")]
        public string ResultDescription { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
