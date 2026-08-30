using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Authentication;

namespace FinTrack.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResponse>> Register(RegisterRequest request);
        Task<Result<AuthenticationResponse>> Login(LoginRequest request);
        Task<Result<AuthenticationResponse>> RefreshToken(RefreshTokenRequest request);
        Task<Result> Logout(LogoutRequest request);


    }
}