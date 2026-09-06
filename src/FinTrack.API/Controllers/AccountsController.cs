using System.Security.Claims;
using FinTrack.API.Models;
using FinTrack.Application.DTOs.Accounts;
using FinTrack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace FinTrack.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController:ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService=accountService;
        }

        [HttpPost]
        public async Task<IActionResult>CreateAccount(CreateAccountRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);
            var result=await _accountService.CreateAccount(request,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccount(Guid accountId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _accountService.GetAccount(
                accountId,
                userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(
                    result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var userIdClaim = User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _accountService.GetAccounts(userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(
                    result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult>UpdateAccount(Guid accountId, UpdateAccountRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);
            var result=await _accountService.UpdateAccount(accountId, request,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }
        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult>DeactivateteAccount(Guid accountId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);
            var result=await _accountService.DeactivateAccount(accountId,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return NoContent();
        }

    }
}