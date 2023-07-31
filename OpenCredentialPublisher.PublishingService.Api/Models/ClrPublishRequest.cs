using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public class ClrPublishRequest
    {
        [JsonProperty("identity", Required = Required.Always)]
        public ClrPublishRequestIdentity Identity { get; set; }

        [JsonProperty("clr", Required = Required.Always)]
        public ClrDType Clr { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

}
