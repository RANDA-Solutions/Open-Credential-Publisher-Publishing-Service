// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var class1 = Class1.FromJson(jsonString);

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class RubricCriterionLevel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("alignment")]
        public Alignment[] Alignment { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("points")]
        public string Points { get; set; }
    }
}
