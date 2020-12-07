using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class DiscoveryDocumentDType
    {
        [JsonProperty("authorizationUrl", Required = Required.Always)]
        public string AuthorizationUrl { get; set; }
        [JsonProperty("image", Required = Required.Default)]
        public string Image { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("privacyPolicyUrl", Required = Required.Always)]
        public string PrivacyPolicyUrl { get; set; }
        [JsonProperty("registrationUrl", Required = Required.Always)]
        public string RegistrationUrl { get; set; }
        [JsonProperty("scopesOffered", Required = Required.Always)]
        public List<string> ScopesOffered { get; set; }
        [JsonProperty("termsOfServiceUrl", Required = Required.Always)]
        public string TermsOfServiceUrl { get; set; }
        [JsonProperty("tokenUrl", Required = Required.Always)]
        public string TokenUrl { get; set; }
    }

}
