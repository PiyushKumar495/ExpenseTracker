using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction?> FindById(Guid id);
        Task<List<Transaction>> GetByUserId(Guid userId);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<List<Transaction>> GetByTransferId(Guid transferId);
    }
}