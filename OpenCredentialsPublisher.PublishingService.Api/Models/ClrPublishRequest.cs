using Newtonsoft.Json;
using OpenCredentialsPublisher.Credentials.Clrs.Clr;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public class ClrPublishRequest
    {
        [JsonProperty("identity", Required = Required.Always)]
        public ClrPublishRequestIdentity Identity { get; set; }

        [JsonProperty("clr", Required = Required.Always)]
        public ClrDType Clr { get; set; }
    }

}
