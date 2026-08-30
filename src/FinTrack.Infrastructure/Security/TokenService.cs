using FinTrack.Application.Configuration;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using Microsoft.Extensions.Options;

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
            throw new NotImplementedException();
        }
        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}