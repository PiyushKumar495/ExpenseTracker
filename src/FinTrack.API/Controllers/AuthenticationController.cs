using FinTrack.API.Models;
using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService=authenticationService;
        }

        [HttpPost("register")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult>Register(RegisterRequest request)
        {
            var result=await _authenticationService.Register(request);
            // if(!result.IsSuccess)
            // {
            //     return BadRequest(result.Error);
            // }
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return StatusCode(StatusCodes.Status201Created, result.Value);
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult>Login(LoginRequest request)
        {
            var result=await _authenticationService.Login(request);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult>RefreshToken(RefreshTokenRequest request)
        {
            var result=await _authenticationService.RefreshToken(request);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPost("logout")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult>Logout(LogoutRequest request)
        {
            var result=await _authenticationService.Logout(request);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return NoContent();
        }
        
        
        
        
    }
}