using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace OpenCredentialPublisher.PublishingService.Api
{

    public class DynamicClientRegistrationService : IDynamicClientRegistrationService
    {
        private readonly ConfigurationDbContext _context;

        public DynamicClientRegistrationService(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task<OcpDynamicClientRegistrationResult> RegisterClientAsync(OcpDynamicClientRegistrationRequest model)
        {
            var clientId = Guid.NewGuid().ToString("d");

            var secret = PasswordGenerator.GetRandomAlphanumericString(40);

            var scopes = model.Scope?.Split(' ').ToList();

            var client = new Client
            {
                ClientId = clientId,
                ClientName = model.ClientName,
                ClientUri = model.ClientUri,
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ClientCredentials.Append("refresh_token").ToList(),
                ClientSecrets = new List<Secret> { new Secret(secret.Sha256()) },
                AllowedScopes = scopes,
            };

            _context.Clients.Add(client.ToEntity());

            await _context.SaveChangesAsync();

            return new OcpDynamicClientRegistrationResult()
            {
                ClientId = client.ClientId,   
                ClientName = client.ClientName,
                ClientSecret = secret,
                ClientIdIssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ClientSecretExpiresAt = 0,
                ClientUri = client.ClientUri,
                GrantTypes = client.AllowedGrantTypes,
                Scope = string.Join(' ', client.AllowedScopes),
                TokenEndpointAuthenticationMethod = EndpointAuthenticationMethods.BasicAuthentication
            };
        }
    }

}
