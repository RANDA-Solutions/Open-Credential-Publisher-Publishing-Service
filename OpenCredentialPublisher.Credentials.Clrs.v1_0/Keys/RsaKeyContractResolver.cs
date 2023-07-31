using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Keys
{
    /// <summary>
    /// </summary>
    public class RsaKeyContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.Ignored = false;

            return property;
        }
    }
}
