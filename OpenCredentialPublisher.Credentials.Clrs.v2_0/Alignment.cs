using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Alignment
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; } = new[] { "Alignment" };

        [JsonProperty("targetCode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetCode")]
        public string TargetCode { get; set; }

        [JsonProperty("targetDescription", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetDescription")]
        public string TargetDescription { get; set; }

        [JsonProperty("targetName", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetName")]
        public string TargetName { get; set; }

        [JsonProperty("targetFramework", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetFramework")]
        public string TargetFramework { get; set; }

        [JsonProperty("targetType", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetType")]
        public string TargetType { get; set; }

        [JsonProperty("targetUrl", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("targetUrl")]
        public string TargetUrl { get; set; }
    }
}
