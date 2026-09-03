using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FinTrack.Application.Configuration;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace FinTrack.Infrastructure.Security
{
    public class TokenService: ITokenService
    {
        private readonly JwtSettings _jwt;
        public TokenService(IOptions<JwtSettings> jwt)
        {
            _jwt=jwt.Value;
        }
        public string GenerateAccessToken(User user)
        {
            var claims=new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
                
            };

            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));

            var credentials=new SigningCredentials(
                key,SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken
            (
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_jwt.AccessTokenLifeTime),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}