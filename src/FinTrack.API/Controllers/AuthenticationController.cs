using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.Interfaces;
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
        public async Task<IActionResult>Register(RegisterRequest request)
        {
            var result=await _authenticationService.Register(request);
            if(!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginRequest request)
        {
            var result=await _authenticationService.Login(request);
            if(!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        
    }
}