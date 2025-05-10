using FERSOFT.ERP.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly int _expirationDays;

        public JwtService(IConfiguration configuration)
        {
            _secret = configuration["Jwt:Key"];
            _expirationDays = int.Parse(configuration["Jwt:ExpirationDays"]);
        }

        public string GenerateToken(string userName, IList<string> roles)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (roles == null || roles.Count == 0) throw new ArgumentException("Debe tener al menos un rol.", nameof(roles));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_expirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
