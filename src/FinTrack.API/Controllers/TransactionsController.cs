using System.Security.Claims;
using FinTrack.API.Models;
using FinTrack.Application.DTOs.Transactions;
using FinTrack.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            CreateTransactionRequest request)
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService
                .CreateTransaction(request, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{transactionId:guid}")]
        public async Task<IActionResult> GetTransaction(Guid transactionId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService.GetTransaction(transactionId, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService
                .GetTransactions(userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{transactionId:guid}")]
        public async Task<IActionResult> UpdateTransaction(
            Guid transactionId,
            UpdateTransactionRequest request)
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService
                .UpdateTransaction(transactionId, request, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpDelete("{transactionId:guid}")]
        public async Task<IActionResult> DeactivateTransaction(Guid transactionId)
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService
                .DeactivateTransaction(transactionId, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return NoContent();
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTransfer(CreateTransferRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _transactionService.CreateTransfer(request, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }
    }
}