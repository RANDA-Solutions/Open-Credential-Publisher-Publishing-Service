using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class EndorsedCredential: VerifiableCredential 
    { 
        [JsonProperty("endorsement", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsement")]
        public EndorsementCredential[] Endorsement { get; set; }

        [JsonProperty("endorsementJwt", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("endorsementJwt")]
        public string[] EndorsementJwt { get; set; }
    }
}
