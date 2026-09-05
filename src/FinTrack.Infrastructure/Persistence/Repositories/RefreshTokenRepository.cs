using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly FinTrackDbContext _context;
        public RefreshTokenRepository(FinTrackDbContext context)
        {
            _context=context;
            
        }
        public async Task<RefreshToken> Add(RefreshToken token)
        {
            throw new NotImplementedException();
        }
        public async Task<RefreshToken?> FindByTokenHash(string tokenHash)
        {
            throw new NotImplementedException();
        }
        public async Task Revoke(RefreshToken token)
        {
            throw new NotImplementedException();
        }
    }
}