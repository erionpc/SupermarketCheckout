using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    class JWTSecurityTokenService : ISecurityTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JWTSecurityTokenService(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public SecurityToken GetSecurityToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:TokenExpiryMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        }

        public IEnumerable<Claim> GetSecurityTokenClaims()
        {
            yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

            foreach (var audience in _configuration.GetSection("JWT:ValidAudiences").Get<string[]>())
            {
                yield return new Claim(JwtRegisteredClaimNames.Aud, audience);
            }
        }

        public string SerializeSecurityToken(SecurityToken token)
        {
            return _tokenHandler.WriteToken(token);
        }
    }
}
