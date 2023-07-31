using Newtonsoft.Json;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class CodeMinorFieldDType
    {
        /// <summary>
        /// This should contain the identity of the system that has produced the code minor status code report.
        /// </summary>
        [JsonProperty("imsx_codeMinorFieldName", Required = Required.Always)]
        public string CodeMinorFieldName { get; set; }
        [JsonProperty("imsx_codeMinorFieldValue", Required = Required.Always)]
        public CodeMinorFieldEnum CodeMinorFieldValue { get; set; }
    }

}
