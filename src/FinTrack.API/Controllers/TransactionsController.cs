using FinTrack.API.Helpers;
using FinTrack.API.Models;
using FinTrack.Application.DTOs.Common;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTransaction(
            CreateTransactionRequest request)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

            var result = await _transactionService
                .CreateTransaction(request, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return CreatedAtAction(
                nameof(GetTransaction),
                new { transactionId = result.Value!.Id },
                result.Value
            );
        }

        [HttpGet("{transactionId:guid}")]
        public async Task<IActionResult> GetTransaction(Guid transactionId)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

            var result = await _transactionService.GetTransaction(transactionId, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterRequest request,[FromQuery] PaginationRequest pagination)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }
            var result = await _transactionService
                .GetTransactions(userId, request,pagination);

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
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

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
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTransfer(CreateTransferRequest request)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

            var result = await _transactionService.CreateTransfer(request, userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping
                    .GetStatusCode(result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return StatusCode(StatusCodes.Status201Created, result.Value);
        }
    }
}