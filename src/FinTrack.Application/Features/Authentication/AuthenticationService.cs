using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Interfaces;

namespace FinTrack.Application.Features.Authentication
{
    public class AuthenticationService:IAuthenticationService
    {
        public async Task<AuthenticationResponse> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthenticationResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task Logout(LogoutRequest request)
        {
            throw new NotImplementedException();
        }


    }
}