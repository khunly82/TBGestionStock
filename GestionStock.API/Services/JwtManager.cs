using GestionStock.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GestionStock.API.Services
{
    public class JwtManager(IConfiguration configuration)
    {
        public string GenerateToken(string username, RoleEnum roles)
        {
            JwtSecurityToken token = new(
                "api", // issuer
                null, // audience
                [
                    new Claim(ClaimTypes.NameIdentifier, username),
                    ..Enum.GetValues<RoleEnum>() // recupérer tous les roles existants
                        .Where(r => roles.HasFlag(r)) // filter les roles
                        .Select(r => new Claim(ClaimTypes.Role, r.ToString()))
                ], // claims
                DateTime.UtcNow, // date de debut de validité
                DateTime.UtcNow.AddYears(1), // date de fin de validité
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"))),
                    SecurityAlgorithms.HmacSha256
                ) // signature
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
