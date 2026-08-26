using Coopad.Administration.Api.Configuration;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coopad.Administration.Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(
            IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new(
                    ClaimTypes.Name,
                    user.Username),

                new(
                    "displayName",
                    user.DisplayName)
            };

            foreach (var userRole in user.UserRoles)
            {
                if (!userRole.Role.IsActive)
                    continue;

                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        userRole.Role.Name));

                foreach (var rolePermission
                    in userRole.Role.RolePermissions)
                {
                    if (!rolePermission.Permission.IsActive)
                        continue;

                    claims.Add(
                        new Claim(
                            "permission",
                            rolePermission.Permission.Name));
                }
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _settings.ExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
