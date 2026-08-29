using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> Add(RefreshToken token);
        Task<RefreshToken?> FindByTokenHash(string tokenHash);
        Task Revoke(RefreshToken token);


    }
}