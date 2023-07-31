using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class AddressDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("addressCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressCountry { get; set; }
        [JsonProperty("addressLocality", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressLocality { get; set; }
        [JsonProperty("addressRegion", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressRegion { get; set; }
        [JsonProperty("postalCode", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }
        [JsonProperty("postOfficeBoxNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PostOfficeBoxNumber { get; set; }
        [JsonProperty("streetAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
