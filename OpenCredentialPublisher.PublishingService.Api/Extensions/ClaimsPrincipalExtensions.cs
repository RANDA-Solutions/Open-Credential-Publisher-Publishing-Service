using System.Security.Claims;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public static class ClaimsPrincipalExtensions
    {
        public static string ClientId(this ClaimsPrincipal user)
        {
            return user.FindFirst(u => u.Type == "client_id")?.Value;
        }

        public static string AccessKeyBaseUri(this ClaimsPrincipal user)
        {
            return user.FindFirst(u => u.Type.EndsWith(ClaimConstants.AccessKeyBaseUri))?.Value;
        }

        public static string PushUri(this ClaimsPrincipal user)
        {
            return user.FindFirst(u => u.Type.EndsWith(ClaimConstants.PushUri))?.Value;
        }
    }
 
}
