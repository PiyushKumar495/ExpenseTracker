using FinTrack.Application.DTOs.Common;
using FinTrack.Application.DTOs.Transactions;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction?> FindById(Guid id);
        Task<(List<Transaction>Transactions,int TotalCount)> GetByUserId(Guid userId,TransactionFilterRequest request,PaginationRequest pagination);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<List<Transaction>> GetByTransferId(Guid transferId);
    }
}