using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class RevocationListDType
    {
        /// <summary>
        /// The URI of the RevocationList document. Used during Signed verification.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        /// <summary>
        /// The id of the Issuer.
        /// </summary>
        [JsonProperty("issuer", Required = Required.Always)]
        public string Issuer { get; set; }
        /// <summary>
        /// The id of an assertion that has been revoked.
        /// </summary>
        [JsonProperty("revokedAssertions", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RevokedAssertions { get; set; }
    }

}
