using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Achievement
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("alignment")]
        public Alignment[] Alignment { get; set; }

        [JsonProperty("achievementType", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("achievementType")]
        [Newtonsoft.Json.JsonConverter(typeof(EnumExtStringConverter<v2_0.Clr.AchievementTypeEnum>))]
        public string AchievementType { get; set; }

        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("creator")]
        public Profile Creator { get; set; }

        [JsonProperty("creditsAvailable", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("creditsAvailable")]
        public float? CreditsAvailable { get; set; }

        [JsonProperty("criteria", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("criteria")]
        public Criteria Criteria { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("endorsement", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsement")]
        public EndorsementCredential[] Endorsement { get; set; }

        [JsonProperty("endorsementJwt", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsementJwt")]
        public string[] EndorsementJwt { get; set; }

        [JsonProperty("fieldOfStudy", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("fieldOfStudy")]
        public string FieldOfStudy { get; set; }

        [JsonProperty("humanCode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("humanCode")]
        public string HumanCode { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonProperty("@language", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("@language")]
        public string Language { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("otherIdentifier", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("otherIdentifier")]
        public IdentifierEntry[] OtherIdentifier { get; set; }

        [JsonProperty("related", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("related")]
        public Related[] Related { get; set; }

        [JsonProperty("resultDescription", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("resultDescription")]
        public ResultDescription[] ResultDescription { get; set; }

        [JsonProperty("specialization", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("specialization")]
        public string Specialization { get; set; }

        [JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("tag")]
        public string[] Tag { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
