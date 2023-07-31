using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Converters.Newtonsoft;
using System;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class AchievementSubject : CredentialSubject, ICredentialSubject
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("activityEndDate", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("activityEndDate")]
        [Newtonsoft.Json.JsonConverter(typeof(DateConverter<DateTime>), "o"), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.DateConverter))]
        public DateTime? ActivityEndDate { get; set; }

        [JsonProperty("activityStartDate", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("activityStartDate")]
        [Newtonsoft.Json.JsonConverter(typeof(DateConverter<DateTime>), "o"), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.DateConverter))]
        public DateTime? ActivityStartDate { get; set; }

        [JsonProperty("creditsEarned", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("creditsEarned")]
        public float? CreditsEarned { get; set; }

        [JsonProperty("achievement", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("achievement")]
        public Achievement Achievement { get; set; }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("identifier")]
        public IdentityObject[] Identifier { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonProperty("licenseNumber", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("licenseNumber")]
        public string LicenseNumber { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("result")]
        public Result[] Result { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("source")]
        public Profile Source { get; set; }

        [JsonProperty("term", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("term")]
        public string Term { get; set; }


    }
}
