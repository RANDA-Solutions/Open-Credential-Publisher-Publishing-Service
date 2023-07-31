using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Profile
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("endorsement", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsement")]
        public EndorsementCredential[] Endorsement { get; set; }

        [JsonProperty("endorsementJwt", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsementJwt")]
        public string[] EndorsementJwt { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonProperty("otherIdentifier", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("otherIdentifier")]
        public IdentifierEntry[] OtherIdentifier { get; set; }

        [JsonProperty("official", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("official")]
        public string Official { get; set; }

        [JsonProperty("parentOrg", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("parentOrg")]
        public Profile ParentOrg { get; set; }

        [JsonProperty("familyName", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("givenName", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("additionalName", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("additionalName")]
        public string AdditionalName { get; set; }

        [JsonProperty("patronymicName", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("patronymicName")]
        public string PatronymicName { get; set; }

        [JsonProperty("honorificPrefix", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("honorificPrefix")]
        public string HonorificPrefix { get; set; }

        [JsonProperty("honorificSuffix", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("honorificSuffix")]
        public string HonorificSuffix { get; set; }

        [JsonProperty("familyNamePrefix", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("familyNamePrefix")]
        public string FamilyNamePrefix { get; set; }

        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
