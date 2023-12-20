using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(){
                UserClaims = {"name"}
            },
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("carsties.auction", "Auction api service full access"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = { "openid", "profile", "carsties.auction" },
                RedirectUris = { "https://www.getpostmane.com/oauth2/callback" }, // Not going to be used. nore redirection in postman testing 
                ClientSecrets = new Secret[] { new Secret("P@ssw0rd".Sha256()) },
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword }
            }
        };
}
