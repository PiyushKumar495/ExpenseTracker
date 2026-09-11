using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Accounts;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
namespace FinTrack.Application.Features.Authentication
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IAccountRepository accountRepo, IUnitOfWork unitOfWork)
        {
            _accountRepo=accountRepo;
            _unitOfWork=unitOfWork;
        }

        public async Task<Result<AccountResponse>> CreateAccount(CreateAccountRequest request,Guid userId)
        {
            var account=new Account
            {
              UserId=userId,
              Name = request.Name,
              AccountType = request.AccountType,
              OpeningBalance = request.OpeningBalance,
              CurrentBalance = request.OpeningBalance,
              Currency = request.Currency,
              Description = request.Description

            };
            await _accountRepo.AddAccount(account);
            await _unitOfWork.SaveChangesAsync();
            var response = new AccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Name = account.Name,
                AccountType = account.AccountType,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                Currency = account.Currency,
                Description = account.Description,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };

            return new Result<AccountResponse>
            {
                IsSuccess=true,
                Value=response
            };
        }
        public async Task<Result<AccountResponse>> GetAccount(Guid accountId,Guid userId)
        {
            var account=await _accountRepo.FindById(accountId);
            if(account is null || account.UserId!=userId || !account.IsActive)
            {
                return new Result<AccountResponse>
                {
                  IsSuccess=false,
                  Error=new Error
                  {
                      Code="ACCOUNT_NOT_FOUND",
                      Message="Account not found"
                  }
                };
                
            }
            var response = new AccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Name = account.Name,
                AccountType = account.AccountType,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                Currency = account.Currency,
                Description = account.Description,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };

            return new Result<AccountResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<List<AccountResponse>>> GetAccounts(Guid userId)
        {
            var accounts=await _accountRepo.FindByUserId(userId);
            var response = accounts.Select(account => new AccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Name = account.Name,
                AccountType = account.AccountType,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                Currency = account.Currency,
                Description = account.Description,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            }).ToList();

            return new Result<List<AccountResponse>>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<AccountResponse>> UpdateAccount(Guid accountId,UpdateAccountRequest request,Guid userId)
        {
            var account = await _accountRepo.FindById(accountId);

            if (account is null || account.UserId != userId || !account.IsActive)
            {
                return new Result<AccountResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Account not found."
                    }
                };
            }

            account.Name = request.Name;
            account.Currency = request.Currency;
            account.Description = request.Description;
            account.UpdatedAt = DateTime.UtcNow;

            await _accountRepo.UpdateAccount(account);
            await _unitOfWork.SaveChangesAsync();

            var response = new AccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Name = account.Name,
                AccountType = account.AccountType,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                Currency = account.Currency,
                Description = account.Description,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };

            return new Result<AccountResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result> DeactivateAccount(Guid accountId,Guid userId)
        {
            var account = await _accountRepo.FindById(accountId);

            if (account is null || account.UserId != userId || !account.IsActive)
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Account not found."
                    }
                };
            }

            account.IsActive = false;
            account.UpdatedAt = DateTime.UtcNow;

            await _accountRepo.UpdateAccount(account);
            return new Result
            {
                IsSuccess = true,
            };
        }
    }
}