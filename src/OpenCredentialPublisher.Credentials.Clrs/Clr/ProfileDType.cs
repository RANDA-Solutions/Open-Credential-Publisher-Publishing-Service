using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{

    public class SharedProfileDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("publicKey", NullValueHandling = NullValueHandling.Ignore)]
        public CryptographicKeyDType PublicKey { get; set; }
        [JsonProperty("revocationList", NullValueHandling = NullValueHandling.Ignore)]
        public string RevocationList { get; set; }

    }

    public class ProfileDType : SharedProfileDType
    { 
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public AddressDType Address { get; set; }
        [JsonProperty("additionalName", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalName { get; set; }
        [JsonProperty("birthDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? BirthDate { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("familyName", NullValueHandling = NullValueHandling.Ignore)]
        public string FamilyName { get; set; }
        [JsonProperty("givenName", NullValueHandling = NullValueHandling.Ignore)]
        public string GivenName { get; set; }
        [JsonProperty("endorsements", NullValueHandling = NullValueHandling.Ignore)]
        public List<EndorsementDType> Endorsements { get; set; }
        [JsonProperty("identifiers", NullValueHandling = NullValueHandling.Ignore)]
        public List<SystemIdentifierDType> Identifiers { get; set; }
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("official", NullValueHandling = NullValueHandling.Ignore)]
        public string Official { get; set; }
        [JsonProperty("parentOrg", NullValueHandling = NullValueHandling.Ignore)]
        public ProfileDType ParentOrg { get; set; }
        [JsonProperty("signedEndorsements", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SignedEndorsements { get; set; }
        [JsonProperty("sourcedId", NullValueHandling = NullValueHandling.Ignore)]
        public string SourcedId { get; set; }
        [JsonProperty("studentId", NullValueHandling = NullValueHandling.Ignore)]
        public string StudentId { get; set; }
        [JsonProperty("telephone", NullValueHandling = NullValueHandling.Ignore)]
        public string Telephone { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("verification", NullValueHandling = NullValueHandling.Ignore)]
        public VerificationDType Verification { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
