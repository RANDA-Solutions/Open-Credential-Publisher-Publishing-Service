using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class CodeMinorDType
    {
        [JsonProperty("imsx_codeMinorField", Required = Required.Always)]
        public List<CodeMinorFieldDType> CodeMinorField { get; set; }
    }

}
