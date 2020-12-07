using Newtonsoft.Json;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public class OcpDynamicClientRegistrationRequest 
    {
        [JsonProperty(PropertyName = "client_name", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientName { get; set; }

        [JsonProperty(PropertyName = "client_uri", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientUri { get; set; }

        [JsonProperty(PropertyName = "token_endpoint_auth_method", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string TokenEndpointAuthenticationMethod { get; set; }

        [JsonProperty(PropertyName = "scope", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string Scope { get; set; }

    }

}
