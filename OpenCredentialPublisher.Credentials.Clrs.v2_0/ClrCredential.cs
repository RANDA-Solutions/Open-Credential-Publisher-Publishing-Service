using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{

    public class ClrCredential: EndorsedCredential
    {
        public ClrCredential()
        {
            Context = new[] {
                "https://www.w3.org/2018/credentials/v1",
                "https://purl.imsglobal.org/spec/clr/v2p0/context.json",
                "https://purl.imsglobal.org/spec/ob/v3p0/context-3.0.2.json",
                "https://w3id.org/security/suites/ed25519-2020/v1"
            };

            Type = new[]
            {
                "VerifiableCredential",
                "ClrCredential"
            };
        }

        [JsonProperty("credentialSubject", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("credentialSubject")]
        public new ClrSubject CredentialSubject { get; set; }

        [JsonProperty("partial", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("partial")]
        public bool Partial { get; set; }

        [JsonProperty("awardedDate", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("awardedDate")]
        public string AwardedDate { get; set; }
    }
}
