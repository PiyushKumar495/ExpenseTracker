using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Accounts;

namespace FinTrack.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountResponse>> CreateAccount(CreateAccountRequest request,Guid userId);
        Task<Result<AccountResponse>> GetAccount(Guid accountId,Guid userId);
        Task<Result<List<AccountResponse>>> GetAccounts(Guid userId);
        Task<Result<AccountResponse>> UpdateAccount(Guid accountId,UpdateAccountRequest request,Guid userId);
    }
}