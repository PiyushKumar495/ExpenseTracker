using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByEmail(string email);
        Task<User?> FindById(Guid id);
        Task<bool> ExistsByEmail(string email);
        Task<User>AddUser(User user);
        Task<User>UpdateUser(User user);

    }
}