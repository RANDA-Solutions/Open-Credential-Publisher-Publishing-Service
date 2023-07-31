using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public class CodeMinorDType
    {
        [JsonProperty("imsx_codeMinorField", Required = Required.Always)]
        public List<CodeMinorFieldDType> CodeMinorField { get; set; }
    }

}
