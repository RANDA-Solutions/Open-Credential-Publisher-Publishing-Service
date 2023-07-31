using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public class Address
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
        public string[] Type { get; set; }

        [JsonProperty("addressCountry", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("addressCountryCode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("addressCountryCode")]
        public string AddressCountryCode { get; set; }

        [JsonProperty("addressRegion", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("addressRegion")]
        public string AddressRegion { get; set; }

        [JsonProperty("addressLocality", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("addressLocality")]
        public string AddressLocality { get; set; }

        [JsonProperty("streetAddress", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("postOfficeBoxNumber", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("postOfficeBoxNumber")]
        public string PostOfficeBoxNumber { get; set; }

        [JsonProperty("postalCode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("geo")]
        public GeoCoordinates Geo { get; set; }
    }
}
