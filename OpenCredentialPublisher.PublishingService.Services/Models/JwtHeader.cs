using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.PublishingService.Services
{
    public class JwtHeader
    {
        [JsonPropertyName("alg")]
        public string Alg { get; set; }
        [JsonPropertyName("b64")]
        public bool B64 { get; set; } = true;
        [JsonPropertyName("crit")]
        public string[] Crit { get; set; }
        [JsonPropertyName("enc")]
        public string Enc { get; set; }
        [JsonPropertyName("kid")]
        public string Kid { get; set; }
        [JsonPropertyName("typ")]
        public string Type { get; set; }
    }

}
