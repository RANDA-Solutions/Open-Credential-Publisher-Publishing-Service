using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class ClrSetDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = "ClrSet";
        [JsonProperty("clrs", NullValueHandling = NullValueHandling.Ignore)]
        public List<ClrDType> Clrs { get; set; }
        [JsonProperty("signedClrs", NullValueHandling = NullValueHandling.Ignore)]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public List<string> SignedClrs { get; set; }
    }

}
