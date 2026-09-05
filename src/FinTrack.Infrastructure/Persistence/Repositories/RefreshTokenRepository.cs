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
        public async Task<RefreshToken> AddRefreshToken(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }
        public async Task<RefreshToken?> FindByTokenHash(string tokenHash)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t=>t.TokenHash==tokenHash);
        }
        public async Task Revoke(RefreshToken token)
        {
            token.RevokedAt=DateTime.UtcNow;
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
        public async Task Update(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}