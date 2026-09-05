using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddRefreshToken(RefreshToken token);
        Task<RefreshToken?> FindByTokenHash(string tokenHash);
        Task Revoke(RefreshToken token);
        Task Update(RefreshToken token);

    }
}