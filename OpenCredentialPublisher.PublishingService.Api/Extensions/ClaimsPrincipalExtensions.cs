using System.Security.Claims;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public static class ClaimsPrincipalExtensions
    {
        public static string ClientId(this ClaimsPrincipal user)
        {
            return user.FindFirst(u => u.Type == "client_id")?.Value;
        }
    }
 
}
