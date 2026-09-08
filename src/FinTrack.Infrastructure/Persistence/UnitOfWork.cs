using FinTrack.Application.Interfaces;
using FinTrack.Infrastructure.Persistence.Context;

namespace FinTrack.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTrackDbContext _context;

        public UnitOfWork(FinTrackDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CurrentTransaction!.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
        }
    }
}