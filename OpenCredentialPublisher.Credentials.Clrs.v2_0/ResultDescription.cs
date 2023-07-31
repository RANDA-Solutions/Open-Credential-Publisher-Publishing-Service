// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var class1 = Class1.FromJson(jsonString);

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class ResultDescription
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; } = new string[] { "ResultDescription" };

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("alignment")]
        public Alignment[] Alignment { get; set; }

        [JsonProperty("allowedValue", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("allowedValue")]
        public string[] AllowedValue { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("requiredLevel", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("requiredLevel")]
        public string RequiredLevel { get; set; }

        [JsonProperty("requiredValue", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("requiredValue")]
        public string RequiredValue { get; set; }

        [JsonProperty("resultType", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("resultType")]
        public string ResultType { get; set; }

        [JsonProperty("rubricCriterionLevel", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("rubricCriterionLevel")]
        public RubricCriterionLevel[] RubricCriterionLevel { get; set; }

        [JsonProperty("valueMax", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("valueMax")]
        public string ValueMax { get; set; }

        [JsonProperty("valueMin", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("valueMin")]
        public string ValueMin { get; set; }
    }
}
