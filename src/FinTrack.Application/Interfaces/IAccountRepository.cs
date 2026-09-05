using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAccount(Account account);
        Task<Account?> FindById(Guid id);
        Task<List<Account>>GetByUserId(Guid userId);
        Task<Account>UpdateAccount(Account account);
    }
}