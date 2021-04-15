using System.Collections;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;

namespace Identity.API.Configuration
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => 
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role.scope",
                    UserClaims = { "role" }
                }
            };

        public static IEnumerable<ApiResource> GetResources() =>
            new List<ApiResource>()
            {
                new ApiResource("book_api", "Book API")
                {
                    Scopes = { "book_api"}
                }
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>()
            {
                new ApiScope("book_api")
            };

        public static IEnumerable<Client> GetClients() => 
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "organisation_spa_client",
                    ClientName = "Organisation SPA",
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,

                    RedirectUris = { "http://localhost:4200" },
                    PostLogoutRedirectUris = { "https://localhost:8001/auth/login" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,

                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "role.scope",
                        "book_api"
                        
                    }
                },
                new Client()
                {
                    ClientId = "book_api_client",
                    ClientName = "Book API"
                }
            };
        
    }
}