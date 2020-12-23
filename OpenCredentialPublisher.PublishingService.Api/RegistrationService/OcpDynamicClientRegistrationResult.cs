using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public class OcpDynamicClientRegistrationResult
    {
        [JsonProperty(PropertyName = "client_id", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "client_secret", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientSecret { get; set; }

        [JsonProperty(PropertyName = "client_id_issued_at", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public long ClientIdIssuedAt { get; set; }

        [JsonProperty(PropertyName = "client_secret_expires_at", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public long ClientSecretExpiresAt { get; set; }

        [JsonProperty(PropertyName = "client_name", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientName { get; set; }

        [JsonProperty(PropertyName = "client_uri", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ClientUri { get; set; }

        [JsonProperty(PropertyName = "token_endpoint_auth_method", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string TokenEndpointAuthenticationMethod { get; set; }

        [JsonProperty(PropertyName = "scope", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "grant_types", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public ICollection<string> GrantTypes { get; set; }
    }

}
