using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class IdentityObject
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string Type { get; set; } = nameof(IdentityObject);

        [JsonProperty("hashed", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("hashed")]
        public bool Hashed { get; set; }

        /// <summary>
        /// A string consisting of algorithm$hash
        /// Supported algorithms are md5 and sha256
        /// Hash must be expressed in hexadecimal
        /// </summary>
        [JsonProperty("identityHash", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("identityHash")]
        public string IdentityHash { get; set; }

        [JsonProperty("identityType", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("identityType")]
        public string IdentityType { get; set; }

        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("salt")]
        public string Salt { get; set; }
    }
}
