using Newtonsoft.Json;
using System;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{
    public class Issuer
    {
        [JsonProperty("id", Order = 1)]
        public String Id { get; set; }

        [JsonProperty("name", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public String Name { get; set; }
    }
}
