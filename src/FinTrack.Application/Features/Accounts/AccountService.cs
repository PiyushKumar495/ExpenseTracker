using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Accounts;
using FinTrack.Application.Interfaces;

namespace FinTrack.Application.Features.Authentication
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo=accountRepo;
        }

        public async Task<Result<AccountResponse>> CreateAccount(CreateAccountRequest request,Guid userId)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<AccountResponse>> GetAccount(Guid accountId,Guid userId)
        {
            
            throw new NotImplementedException();
        }
        public async Task<Result<List<AccountResponse>>> GetAccounts(Guid userId)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<AccountResponse>> UpdateAccount(Guid accountId,UpdateAccountRequest request,Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}