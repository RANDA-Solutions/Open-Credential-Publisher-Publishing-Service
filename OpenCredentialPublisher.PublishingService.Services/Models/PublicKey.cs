using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.PublishingService.Services.Models
{
    public class PublicKey
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonProperty("controller", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("controller")]
        public string Controller { get; set; }
        [JsonProperty("publicKeyMultibase", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("publicKeyMultibase")]
        public string PublicKeyMultibase { get; set; }
    }
}
