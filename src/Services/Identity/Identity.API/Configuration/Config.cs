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
                    UserClaims = { JwtClaimTypes.Role },
                },
                new IdentityResource
                {
                    Name = "book_privilege.scope",
                    UserClaims = { "book_privilege" },
                },
                new IdentityResource()
                {
                    Name = "delivery_privilege.scope",
                    UserClaims = { "delivery_privilege" }
                }
            };

        public static IEnumerable<ApiResource> GetResources() =>
            new List<ApiResource>()
            {
                new ApiResource("book_api", "Book API")
                {
                    UserClaims = {JwtClaimTypes.Role, "book_privilege"},
                    Scopes = { "book_api" }
                },
                new ApiResource("search_api", "Search API")
                {
                    Scopes = { "search_api" }
                },
                new ApiResource("stock_delivery_api", "Stock Delivery API")
                {
                    UserClaims = {JwtClaimTypes.Role, "delivery_privilege"},
                    Scopes = { "stock_delivery_api" }
                },
                new ApiResource("lend_api", "Lend API")
                {
                    Scopes = { "lend_api" }
                }
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>()
            {
                new ApiScope("book_api"),
                new ApiScope("search_api"),
                new ApiScope("stock_delivery_api"),
                new ApiScope("lend_api")
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
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 360,
                    AccessTokenLifetime = 340,
                    RedirectUris = { "http://localhost:4200" },
                    //PostLogoutRedirectUris = { "https://localhost:8001/v1/auth/login" },
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
                        "book_privilege.scope",
                        "delivery_privilege.scope",
                        "book_api",
                        "search_api",
                        "stock_delivery_api",
                        "lend_api" 
                    }
                }
                // new Client()
                // {
                //     ClientId = "book_api_client",
                //     ClientName = "Book API",
                //     ClientSecrets =
                //     {
                //         new Secret("book_api_secret".ToSha256())
                //     },
                //     
                //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                // }
            };
        
    }
}