using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Common;
using FinTrack.Application.DTOs.Transactions;

namespace FinTrack.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<TransactionResponse>> CreateTransaction(CreateTransactionRequest request,Guid userId);

        Task<Result<TransactionResponse>> GetTransaction(Guid transactionId,Guid userId);

        Task<Result<PaginatedTransactionResponse>> GetTransactions(Guid userId, TransactionFilterRequest request,PaginationRequest pagination);

        Task<Result<TransactionResponse>> UpdateTransaction(Guid transactionId,UpdateTransactionRequest request,Guid userId);

        Task<Result> DeactivateTransaction(Guid transactionId,Guid userId);

        Task<Result<List<TransactionResponse>>> CreateTransfer(CreateTransferRequest request,Guid userId);
    }
}