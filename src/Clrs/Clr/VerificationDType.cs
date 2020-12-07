using Newtonsoft.Json;
using OpenCredentialsPublisher.Credentials.Clrs.Converters;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class VerificationDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(EnumAsStringConverter<VerificationTypeEnum>))]
        public VerificationTypeEnum Type { get; set; }
        /// <summary>
        /// The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid.
        /// </summary>
        [JsonProperty("allowedOrigins", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AllowedOrigins { get; set; }
        /// <summary>
        /// The URI of the CryptographicKey with the public key used to verify the signed Assertion. If not present, verifiers will check the publicKey property of the referenced issuer Profile. If present, the URI must match the CryptographicKey id in the issuer Profile as well.
        /// </summary>
        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore)]
        public string Creator { get; set; }
        /// <summary>
        /// The URI fragment that the verification property must start with. Valid Assertions must have an id within this scope. Multiple values allowed, and Assertions will be considered valid if their id starts with one of these values.
        /// </summary>
        [JsonProperty("startsWith", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> StartsWith { get; set; }
        /// <summary>
        /// The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted.
        /// </summary>
        [JsonProperty("verificationProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string VerificationProperty { get; set; }
    }

}
