using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class ClrPayloadDType
    {

        [JsonProperty("clr", NullValueHandling = NullValueHandling.Ignore)]
        public ClrDType Clr { get; set; }
        [JsonProperty("signedClr", NullValueHandling = NullValueHandling.Ignore)]
        [RegularExpression(@"^([A-Za-z0-9-_]{4,})\.([-A-Za-z0-9-_]{4,})\.([A-Za-z0-9-_]{4,})$")]
        public string SignedClr { get; set; }
    }

}
