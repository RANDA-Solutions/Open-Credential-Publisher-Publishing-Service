using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class AchievementDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("achievementType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(EnumExtStringConverter<AchievementTypeEnum>))]
        public string AchievementType { get; set; }

        /// <summary>
        /// Alignment objects describe an alignment between this achievement and a node in an educational framework.
        /// </summary>
        [JsonProperty("alignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<AlignmentDType> Alignments { get; set; }
        [JsonProperty("associations", NullValueHandling = NullValueHandling.Ignore)]
        public List<AssociationDType> Associations { get; set; }
        /// <summary>
        /// Credit hours associated with this entity, or credit hours possible. For example '3.0'.
        /// </summary>
        [JsonProperty("creditsAvailable", NullValueHandling = NullValueHandling.Ignore)]
        public float? CreditsAvailable { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        /// <summary>
        /// Allows endorsers to make specific claims about the Achievement.
        /// </summary>
        [JsonProperty("endorsements", NullValueHandling = NullValueHandling.Ignore)]
        public List<EndorsementDType> Endorsements { get; set; }
        /// <summary>
        /// The code, generally human readable, associated with an achievement.
        /// </summary>
        [JsonProperty("humanCode", NullValueHandling = NullValueHandling.Ignore)]
        public string HumanCode { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        /// <summary>
        /// Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.
        /// </summary>
        [JsonProperty("fieldOfStudy", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldOfStudy { get; set; }
        /// <summary>
        /// IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. 
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        [JsonProperty("issuer", Required = Required.Always)]
        public ProfileDType Issuer { get; set; }
        /// <summary>
        /// Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'.
        /// </summary>
        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }
        [JsonProperty("requirement", NullValueHandling = NullValueHandling.Ignore)]
        public CriteriaDType Requirement { get; set; }
        [JsonProperty("resultDescriptions", NullValueHandling = NullValueHandling.Ignore)]
        public List<ResultDescriptionDType> ResultDescriptions { get; set; }

        /// <summary>
        /// Signed endorsements in JWS Compact Serialization format.
        /// </summary>
        [JsonProperty("signedEndorsements", NullValueHandling = NullValueHandling.Ignore)]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public List<string> SignedEndorsements { get; set; }
        /// <summary>
        /// Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance.
        /// </summary>
        [JsonProperty("specializtion", NullValueHandling = NullValueHandling.Ignore)]
        public string Specialization { get; set; }
        /// <summary>
        /// Tags that describe the type of achievement.
        /// </summary>
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
