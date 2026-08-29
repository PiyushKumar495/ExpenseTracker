using FinTrack.Application.DTOs.Authentication;

namespace FinTrack.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Register(RegisterRequest request);
        Task<AuthenticationResponse> Login(LoginRequest request);
        Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request);
        Task Logout(LogoutRequest request);


    }
}