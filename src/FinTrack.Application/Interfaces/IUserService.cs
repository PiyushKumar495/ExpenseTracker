using FinTrack.Application.DTOs.Authentication;
using FinTrack.Application.DTOs.Users;

namespace FinTrack.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> GetByEmail(string Email);
        Task<UserResponse> GetById(Guid id);
        Task<UserSummaryResponse>AddUser(RegisterRequest user);
        Task<UserResponse>UpdateUser(UpdateUserRequest user);

    }
}