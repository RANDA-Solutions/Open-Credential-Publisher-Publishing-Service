using System.Runtime.Serialization;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr
{
    public enum CodeMinorFieldEnum
    {
        [EnumMember(Value="forbidden")]
        Forbidden,
        [EnumMember(Value= "fullsuccess")]
        FullSuccess,
        [EnumMember(Value= "internal_server_error")]
        InternalServerError,
        [EnumMember(Value= "invalid_data")]
        InvalidData,
        [EnumMember(Value= "invalid_query_parameter")]
        InvalidQueryParameter,
        [EnumMember(Value= "server_busy")]
        ServerBusy,
        [EnumMember(Value= "unauthorizedrequest")]
        UnauthorizedRequest
    }

}
