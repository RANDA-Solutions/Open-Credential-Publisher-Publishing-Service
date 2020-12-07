using Newtonsoft.Json;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public class ClrPublishRequestIdentity
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
    }

}
