using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Features.Authentication
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _tokenRepo;
        private readonly IPasswordHasher _passHash;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenHasher _tokenHash;
        public AuthenticationService(IUserRepository userRepo,IRefreshTokenRepository tokenRepo,IPasswordHasher passHash,ITokenService tokenService,IRefreshTokenHasher tokenHash)
        {
            _userRepo=userRepo;
            _tokenRepo=tokenRepo;
            _passHash=passHash;
            _tokenService=tokenService;
            _tokenHash=tokenHash;
            
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

            string passwordHash=_passHash.HashPassword(request.Password);
            User user= new User
            {
                FirstName=	request.FirstName,
                LastName=	request.LastName,
                Email=	    email,
                PasswordHash=	passwordHash,
                Currency=	request.Currency,
                TimeZone=	request.TimeZone
            };
            await _userRepo.AddUser(user);

            string accessToken=_tokenService.GenerateAccessToken(user);
            string refreshToken=_tokenService.GenerateRefreshToken();
            string refreshTokenHash=_tokenHash.Hash(refreshToken);
            
            RefreshToken token=new RefreshToken
            {
              UserId=user.Id,
              TokenHash=refreshTokenHash,
              //need to configure this
              ExpiresAt=DateTime.UtcNow//+ refreshTokenLifetime
            };

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