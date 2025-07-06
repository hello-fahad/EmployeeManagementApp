using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using WebApp.Model;

namespace WebApi.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this WebApplication app)
        {
            app.MapPost("/account/login", [Tags("Web API - Accounts")] (CredentialViewModel credential, IConfiguration configuration) =>
            {
                if (credential.ClientId == "web-app-001" && credential.ClientSecret == "abcdefghijklmnopqrstuvwxyz" ||
                credential.ClientId == "web-app-002" && credential.ClientSecret == "abcdefghijklmnopqrstuvwxyz")
                {
                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, credential.ClientId));

                    if (credential.ClientId == "web-app-001")
                    {
                        claims.Add(new Claim("Role", "Admin"));
                    }
                    else
                    {
                        claims.Add(new Claim("Role", "User"));
                    }

                    var expiresAt = DateTime.UtcNow.AddMinutes(20);

                    return TypedResults.Ok(new
                    {
                        access_token = CreateToken(claims, expiresAt, configuration),
                        expires_at = expiresAt
                    });
                }
                else
                {
                    return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"ClientId", new[]{$"Errors in ClientId or ClientSecret."} },
                        {"ClientSecret", new[]{$"Errors in ClientId or ClientSecret." } }
                    },
                    statusCode: 401);
                }

            }).WithParameterValidation();
        }


        public static string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ClientAuthentication:SecurityKey"]??string.Empty));

            var singingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var claimDictionary = new Dictionary<string, object>();
            foreach(var claim in claims)
            {
                claimDictionary[claim.Type] = claim.Value;
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claimDictionary,
                Expires = expiresAt,
                SigningCredentials = singingCredentials
            };


            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);

        }
    }
}
