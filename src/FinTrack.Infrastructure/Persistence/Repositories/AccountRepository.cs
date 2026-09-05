using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinTrackDbContext _context;
        public AccountRepository(FinTrackDbContext context)
        {
            _context=context;
            
        }

        public async Task<Account> AddAccount(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<Account?> FindById(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }
        public async Task<List<Account>>GetByUserId(Guid userId)
        {
            return await _context.Accounts.Where(a=>a.UserId==userId).ToListAsync();
        }
        public async Task<Account>UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

    }
}