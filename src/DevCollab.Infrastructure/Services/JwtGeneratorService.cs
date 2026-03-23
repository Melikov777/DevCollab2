using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevCollab.Infrastructure.Services;

public class JwtGeneratorService : IJwtGenerator
{
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtGeneratorService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing")));
        _issuer = config["Jwt:Issuer"] ?? "DevCollab";
        _audience = config["Jwt:Audience"] ?? "DevCollabUsers";
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _issuer,
            Audience = _audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
