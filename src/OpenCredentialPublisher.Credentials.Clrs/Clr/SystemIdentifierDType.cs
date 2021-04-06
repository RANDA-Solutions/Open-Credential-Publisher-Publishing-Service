using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class SystemIdentifierDType
    {
        [Newtonsoft.Json.JsonProperty("type")]
        public string Type { get; set; }

        [ Newtonsoft.Json.JsonProperty("identifier")]
        public string Identifier { get; set; }

        [Newtonsoft.Json.JsonProperty("identifierType")]
        public string IdentifierType { get; set; }


        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
