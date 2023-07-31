using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class CriteriaDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        /// <summary>
        /// Markdown formatted string
        /// </summary>
        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        public string Narrative { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
