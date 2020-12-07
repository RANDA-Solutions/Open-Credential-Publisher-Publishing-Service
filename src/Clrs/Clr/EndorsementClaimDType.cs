using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class EndorsementClaimDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        /// <summary>
        /// Markdown formatted string
        /// </summary>
        [JsonProperty("endorsementComment", NullValueHandling = NullValueHandling.Ignore)]
        public string EndorsementComment { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
