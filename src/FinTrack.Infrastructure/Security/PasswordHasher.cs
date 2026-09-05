using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
namespace FinTrack.Infrastructure.Security
{
    public class PasswordHasher:IPasswordHasher
    {
        private readonly Microsoft.AspNetCore.Identity.PasswordHasher<User>_hasher;
        public PasswordHasher()
        {
            _hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
        }
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }
        public bool VerifyPassword(string password,string passwordHash)
        {
            var result=_hasher.VerifyHashedPassword(
                null!,passwordHash,password
            );
            return result==Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }
    }
}