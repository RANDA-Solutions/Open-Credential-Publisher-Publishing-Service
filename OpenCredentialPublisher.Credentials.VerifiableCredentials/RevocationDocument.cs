using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{
    public class RevocationDocument
    {
        [JsonProperty("statuses")]
        public Dictionary<string, string> Statuses { get; set; }
            = new Dictionary<string, string> {
                { nameof(RevocationReasons.RevokedByIssuer), RevocationReasons.RevokedByIssuer },
                { nameof(RevocationReasons.RevokedByHolder), RevocationReasons.RevokedByHolder },
                { nameof(RevocationReasons.SupersededByIssuer), RevocationReasons.SupersededByIssuer }
            };

        [JsonProperty("revocations")]
        public List<Revocation> Revocations { get; set; }
    }

    public class Revocation
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
