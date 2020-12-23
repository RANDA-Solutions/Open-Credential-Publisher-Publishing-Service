using Newtonsoft.Json;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class AssociationDType
    {
        [JsonProperty("associationType", Required = Required.Always)]
        public AssociationTypeEnum AssociationType { get; set; }
        [JsonProperty("targetId", Required = Required.Always)]
        public string TargetId { get; set; }
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }
    }

}
