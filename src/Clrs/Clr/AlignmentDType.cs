using Newtonsoft.Json;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class AlignmentDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("frameworkName", NullValueHandling = NullValueHandling.Ignore)]
        public string FrameworkName { get; set; }
        [JsonProperty("targetCode", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetCode { get; set; }
        [JsonProperty("targetDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetDescription { get; set; }
        [JsonProperty("targetName", Required = Required.Always)]
        public string TargetName { get; set; }
        [JsonProperty("targetType", Required = Required.Always)]
        [JsonConverter(typeof(EnumExtStringConverter<AlignmentTypeEnum>))]
        public string TargetType { get; set; }
        [JsonProperty("targetUrl", Required = Required.Always)]
        public string TargetUrl { get; set; }
    }

}
