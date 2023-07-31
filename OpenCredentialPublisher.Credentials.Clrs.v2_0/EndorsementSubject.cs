using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class EndorsementSubject: CredentialSubject
    {

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("endorsementComment", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsementComment")]
        public string EndorsementComment { get; set; }
    }
}
