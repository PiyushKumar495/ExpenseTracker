using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Interfaces;

namespace FinTrack.Application.Features.Authentication
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _tokenRepo;
        private readonly IPasswordHasher _passHash;
        private readonly ITokenService _tokenService;
        public AuthenticationService(IUserRepository userRepo,IRefreshTokenRepository tokenRepo,IPasswordHasher passHash,ITokenService tokenService)
        {
            _userRepo=userRepo;
            _tokenRepo=tokenRepo;
            _passHash=passHash;
            _tokenService=tokenService;
            
        }
        public async Task<Result<AuthenticationResponse>> Register(RegisterRequest request)
        {
            string email=request.Email.Trim().ToLowerInvariant();
            var isExist=await _userRepo.ExistsByEmail(email);
            if(isExist)
            {
                return new Result<AuthenticationResponse>
                {IsSuccess=false,Error=new Error {
                    Code = "EMAIL_ALREADY_EXISTS",
                    Message = "Email already exists."
                }};
            }
            throw new NotImplementedException();
        }
        public async Task<Result<AuthenticationResponse>> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<AuthenticationResponse>> RefreshToken(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<Result> Logout(LogoutRequest request)
        {
            throw new NotImplementedException();
        }


    }
}