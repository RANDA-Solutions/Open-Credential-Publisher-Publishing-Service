﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.PublishingService.Api
{
    public static class IdentityServerSetup
    {

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client {
                    ClientId = "testpublisher",
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientName = "Example pub client using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"ocp-publisher" }
                },
                new Client {
                    ClientId = "testwallet",
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientName = "Example wallet client using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"ocp-wallet" }
                }
            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "opencredentials",
                    DisplayName = "OpenCredentials Publisher API",
                    Description = "Allow the application to access OpenCredentials Publisher API on your behalf",
                    Scopes = new List<string> {"ocp-publisher", "ocp-wallet"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("ocp-publisher", "Resource Publisher"),
                new ApiScope("ocp-wallet", "Wallet Consumer"),
            };
        }
    }
}