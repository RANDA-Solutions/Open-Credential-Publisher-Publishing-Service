using Newtonsoft.Json;
using System;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{
    public class CredentialStatus
    {
        [JsonProperty("id", Order = 1)]
        public String Id { get; set; }

        [JsonProperty("type", Order = 2)]
        public String Type { get; set; }
    }
}
