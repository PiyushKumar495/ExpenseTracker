using FinTrack.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;
namespace FinTrack.Infrastructure.Security
{
    public class RefreshTokenHasher:IRefreshTokenHasher
    {
        public string Hash(string refreshToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToHexString(bytes);
        }
        
    }
}