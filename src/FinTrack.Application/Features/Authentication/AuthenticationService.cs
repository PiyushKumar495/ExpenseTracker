using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Application.Configuration;
using Microsoft.Extensions.Options;

namespace FinTrack.Application.Features.Authentication
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly JwtSettings _jwt;
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _tokenRepo;
        private readonly IPasswordHasher _passHash;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenHasher _tokenHash;
        public AuthenticationService(IUserRepository userRepo,IRefreshTokenRepository tokenRepo,IPasswordHasher passHash,ITokenService tokenService,IRefreshTokenHasher tokenHash,IOptions<JwtSettings> jwt)
        {
            _userRepo=userRepo;
            _tokenRepo=tokenRepo;
            _passHash=passHash;
            _tokenService=tokenService;
            _tokenHash=tokenHash;
            _jwt=jwt.Value;
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
              ExpiresAt=DateTime.UtcNow.Add(_jwt.RefreshTokenLifetime)
            };

            await _tokenRepo.AddRefreshToken(token);

            var userSummary=new UserSummaryResponse
            {
              Id=user.Id,
              FirstName=user.FirstName,
              LastName=user.LastName,
              Email=user.Email,
              Currency=user.Currency,
              TimeZone=user.TimeZone  
            };
            var response = new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(_jwt.AccessTokenLifetime),
                User = userSummary
            };

            return new Result<AuthenticationResponse>
            {
                IsSuccess=true,
                Value=response
            };
        }
        public async Task<Result<AuthenticationResponse>> Login(LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user =await _userRepo.FindByEmail(email);
            if(user is null)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess=false,
                    Error=new Error
                    {
                        Code="INVALID_CREDENTIALS",
                        Message="Invalid Email Or Password"
                    }
                };
            }
            var isPasswordValid=_passHash.VerifyPassword(request.Password,user.PasswordHash);
            if(!isPasswordValid)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess=false,
                    Error=new Error
                    {
                        Code="INVALID_CREDENTIALS",
                        Message="Invalid Email Or Password"
                    }
                };
            }
            if(!user.IsActive)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess=false,
                    Error=new Error
                    {
                        Code="USER_INACTIVE",
                        Message="User account is not active"
                    }
                };
            }

            string accessToken = _tokenService.GenerateAccessToken(user);
            string refreshToken = _tokenService.GenerateRefreshToken();
            string refreshTokenHash = _tokenHash.Hash(refreshToken);

            var token=new RefreshToken
            {
                UserId=user.Id,
                TokenHash=refreshTokenHash,
                ExpiresAt=DateTime.UtcNow.Add(_jwt.RefreshTokenLifetime)
            };
            await _tokenRepo.AddRefreshToken(token);

            var userSummary= new UserSummaryResponse
            {
                Id=user.Id,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                Currency=user.Currency,
                TimeZone=user.TimeZone 
            };

            var response = new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(_jwt.AccessTokenLifetime),
                User = userSummary
            };
            return new Result<AuthenticationResponse>
            {
              IsSuccess=true,
              Value=response  
            };
        }
        public async Task<Result<AuthenticationResponse>> RefreshToken(RefreshTokenRequest request)
        {
            var tokenHash=_tokenHash.Hash(request.RefreshToken);
            var token=await _tokenRepo.FindByTokenHash(tokenHash);

            if (token is null)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_REFRESH_TOKEN",
                        Message = "Invalid refresh token."
                    }
                };
            }
            if(token.ExpiresAt<=DateTime.UtcNow)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "REFRESH_TOKEN_EXPIRED",
                        Message = "Refresh Token has expired"
                    }
                };
            }
            if(token.RevokedAt.HasValue)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "REFRESH_TOKEN_REVOKED",
                        Message = "Refresh Token has been revoked"
                    }
                };
            }
            var user=await _userRepo.FindById(token.UserId);
            if (user is null || !user.IsActive)
            {
                return new Result<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_REFRESH_TOKEN",
                        Message = "Invalid refresh token."
                    }
                };
            }

            await _tokenRepo.Revoke(token);

            string accessToken=_tokenService.GenerateAccessToken(user);
            string refreshToken=_tokenService.GenerateRefreshToken();
            string refreshTokenHash=_tokenHash.Hash(refreshToken);

            var newToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                ExpiresAt = DateTime.UtcNow.Add(_jwt.RefreshTokenLifetime)
            };
            token.ReplacedByTokenId = newToken.Id;
            await _tokenRepo.Update(token);
            
            var userSummary = new UserSummaryResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Currency = user.Currency,
                TimeZone = user.TimeZone
            };
            var response = new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(_jwt.AccessTokenLifetime),
                User = userSummary
            };
            return new Result<AuthenticationResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result> Logout(LogoutRequest request)
        {
            var tokenHash = _tokenHash.Hash(request.RefreshToken);
            var token = await _tokenRepo.FindByTokenHash(tokenHash);
            if (token is null)
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_REFRESH_TOKEN",
                        Message = "Invalid refresh token."
                    }
                };
            }
            await _tokenRepo.Revoke(token);
            return new Result
            {
                IsSuccess = true
            };
        }


    }
}