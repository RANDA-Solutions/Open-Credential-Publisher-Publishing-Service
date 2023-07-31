using JsonSubTypes;
using Newtonsoft.Json;
using System;

namespace OpenCredentialPublisher.Credentials.VerifiableCredentials
{

    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(ClrSubject), "Clr")]
    [JsonSubtypes.KnownSubType(typeof(ClrSetSubject), "ClrSet")]
    public interface ICredentialSubject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        String Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        String Type { get; set; }
    }
    public abstract class CredentialSubject: ICredentialSubject
    {
        public String Id { get; set; }
        public String Type { get; set; }
    }
}
