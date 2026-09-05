using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinTrackDbContext _context;
        public UserRepository(FinTrackDbContext context)
        {
            _context=context;
            
        }
        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Email==email);
        }
        public async Task<User?> FindById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Id==id);
        }
        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(u=>u.Email==email);
        }
        public async Task<User>AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User>UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}