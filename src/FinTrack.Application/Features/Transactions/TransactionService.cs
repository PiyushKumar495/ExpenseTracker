using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Transactions;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces
{
    public class TransactionService:ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ICategoryRepository _categoryRepo;
        public TransactionService(ITransactionRepository transactionRepo,IAccountRepository accountRepo,ICategoryRepository categoryRepo)
        {
            _transactionRepo=transactionRepo;
            _accountRepo=accountRepo;
            _categoryRepo=categoryRepo;
        }
        public async Task<Result<TransactionResponse>> CreateTransaction(CreateTransactionRequest request,Guid userId)
        {
            var account = await _accountRepo.FindById(request.AccountId);

            if (account is null || account.UserId != userId)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Account not found."
                    }
                };
            }

            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepo.FindById(request.CategoryId.Value);

                if (category is null ||
                    (!category.IsSystemCategory && category.UserId != userId))
                {
                    return new Result<TransactionResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "CATEGORY_NOT_FOUND",
                            Message = "Category not found."
                        }
                    };
                }
            }

            var transaction = new Transaction
            {
                UserId = userId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                TransactionType = request.TransactionDirection == Domain.Enums.TransactionDirection.In
                    ? Domain.Enums.TransactionType.Income
                    : Domain.Enums.TransactionType.Expense,
                TransactionDirection = request.TransactionDirection,
                Merchant = request.Merchant,
                Description = request.Description,
                TransactionDate = request.TransactionDate
            };

            if (request.TransactionDirection == Domain.Enums.TransactionDirection.In)
            {
                account.CurrentBalance += request.Amount;
            }
            else
            {
                account.CurrentBalance -= request.Amount;
            }

            account.UpdatedAt = DateTime.UtcNow;

            await _transactionRepo.AddTransaction(transaction);
            await _accountRepo.UpdateAccount(account);

            var response = new TransactionResponse
            {
                AccountId = account.Id,
                AccountName = account.Name,
                CategoryId = transaction.CategoryId,
                CategoryName = request.CategoryId.HasValue
                    ? (await _categoryRepo.FindById(request.CategoryId.Value))?.Name
                    : null,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionDirection = transaction.TransactionDirection,
                Merchant = transaction.Merchant,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                CreatedAt = transaction.CreatedAt
            };

            return new Result<TransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }

        public async Task<Result<TransactionResponse>> GetTransaction(Guid transactionId,Guid userId)
        {
            var transaction = await _transactionRepo.FindById(transactionId);
            if (transaction is null || transaction.UserId != userId || !transaction.IsActive)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "TRANSACTION_NOT_FOUND",
                        Message = "Transaction not found."
                    }
                };
            }
            var account = await _accountRepo.FindById(transaction.AccountId);
            var category = transaction.CategoryId.HasValue? await _categoryRepo.FindById(transaction.CategoryId.Value): null;
            
            var response = new TransactionResponse
            {
                AccountId = transaction.AccountId,
                AccountName = account!.Name,
                CategoryId = transaction.CategoryId,
                CategoryName = category?.Name,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionDirection = transaction.TransactionDirection,
                Merchant = transaction.Merchant,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                CreatedAt = transaction.CreatedAt
            };

            return new Result<TransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }

        public async Task<Result<List<TransactionResponse>>> GetTransactions(Guid userId)
        {
            var transactions = await _transactionRepo.GetByUserId(userId);

            var response = new List<TransactionResponse>();

            foreach (var transaction in transactions)
            {
                var account = await _accountRepo.FindById(transaction.AccountId);

                var category = transaction.CategoryId.HasValue
                    ? await _categoryRepo.FindById(transaction.CategoryId.Value)
                    : null;

                response.Add(new TransactionResponse
                {
                    Id = transaction.Id,
                    AccountId = transaction.AccountId,
                    AccountName = account!.Name,
                    CategoryId = transaction.CategoryId,
                    CategoryName = category?.Name,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType,
                    TransactionDirection = transaction.TransactionDirection,
                    Merchant = transaction.Merchant,
                    Description = transaction.Description,
                    TransactionDate = transaction.TransactionDate,
                    CreatedAt = transaction.CreatedAt
                });
            }

            return new Result<List<TransactionResponse>>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<TransactionResponse>> UpdateTransaction(Guid transactionId,UpdateTransactionRequest request,Guid userId)
        {
            var transaction = await _transactionRepo.FindById(transactionId);

            if (transaction is null || transaction.UserId != userId)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "TRANSACTION_NOT_FOUND",
                        Message = "Transaction not found."
                    }
                };
            }

            var account = await _accountRepo.FindById(transaction.AccountId);

            if (account is null || account.UserId != userId)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Account not found."
                    }
                };
            }

            if (request.AccountId != transaction.AccountId)
            {
                var newAccount = await _accountRepo.FindById(request.AccountId);

                if (newAccount is null || newAccount.UserId != userId)
                {
                    return new Result<TransactionResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "ACCOUNT_NOT_FOUND",
                            Message = "Account not found."
                        }
                    };
                }
            }

            // Reverse the old transaction
            if (transaction.TransactionDirection == Domain.Enums.TransactionDirection.In)
            {
                account.CurrentBalance -= transaction.Amount;
            }
            else
            {
                account.CurrentBalance += transaction.Amount;
            }

            var targetAccount = account;

            // If account is being changed, apply the new transaction to the new account
            if (request.AccountId != transaction.AccountId)
            {
                targetAccount = await _accountRepo.FindById(request.AccountId);

                if (targetAccount is null)
                {
                    return new Result<TransactionResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "ACCOUNT_NOT_FOUND",
                            Message = "Account not found."
                        }
                    };
                }
            }

            // Apply the new transaction
            if (request.TransactionDirection == Domain.Enums.TransactionDirection.In)
            {
                targetAccount.CurrentBalance += request.Amount;
            }
            else
            {
                targetAccount.CurrentBalance -= request.Amount;
            }

            // Validate category
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepo.FindById(request.CategoryId.Value);

                if (category is null ||
                    (!category.IsSystemCategory && category.UserId != userId))
                {
                    return new Result<TransactionResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "CATEGORY_NOT_FOUND",
                            Message = "Category not found."
                        }
                    };
                }
            }

            // Update transaction
            transaction.AccountId = request.AccountId;
            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.TransactionDirection = request.TransactionDirection;
            transaction.TransactionType =
                request.TransactionDirection == Domain.Enums.TransactionDirection.In
                    ? Domain.Enums.TransactionType.Income
                    : Domain.Enums.TransactionType.Expense;
            transaction.Merchant = request.Merchant;
            transaction.Description = request.Description;
            transaction.TransactionDate = request.TransactionDate;
            transaction.UpdatedAt = DateTime.UtcNow;

            account.UpdatedAt = DateTime.UtcNow;
            targetAccount.UpdatedAt = DateTime.UtcNow;

            await _transactionRepo.UpdateTransaction(transaction);
            await _accountRepo.UpdateAccount(account);

            if (targetAccount.Id != account.Id)
            {
                await _accountRepo.UpdateAccount(targetAccount);
            }

            var updatedCategory = transaction.CategoryId.HasValue
                ? await _categoryRepo.FindById(transaction.CategoryId.Value)
                : null;

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                AccountId = targetAccount.Id,
                AccountName = targetAccount.Name,
                CategoryId = transaction.CategoryId,
                CategoryName = updatedCategory?.Name,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionDirection = transaction.TransactionDirection,
                Merchant = transaction.Merchant,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                CreatedAt = transaction.CreatedAt
            };

            return new Result<TransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }

        public async Task<Result> DeactivateTransaction(Guid transactionId,Guid userId)
        {
            var transaction = await _transactionRepo.FindById(transactionId);

            if (transaction is null || transaction.UserId != userId)
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "TRANSACTION_NOT_FOUND",
                        Message = "Transaction not found."
                    }
                };
            }

            var account = await _accountRepo.FindById(transaction.AccountId);

            if (account is null || account.UserId != userId)
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

            // Reverse the transaction's effect on the account balance
            if (transaction.TransactionDirection == Domain.Enums.TransactionDirection.In)
            {
                account.CurrentBalance -= transaction.Amount;
            }
            else
            {
                account.CurrentBalance += transaction.Amount;
            }

            account.UpdatedAt = DateTime.UtcNow;

            // Mark transaction as inactive
            transaction.IsActive = false;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _transactionRepo.UpdateTransaction(transaction);
            await _accountRepo.UpdateAccount(account);

            return new Result
            {
                IsSuccess = true
            };
        }

        public async Task<Result<List<TransactionResponse>>> CreateTransfer(CreateTransferRequest request,Guid userId)
        {
            var fromAccount = await _accountRepo.FindById(request.FromAccountId);
            if (fromAccount is null || fromAccount.UserId != userId || !fromAccount.IsActive)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Source account not found."
                    }
                };
            }

            var toAccount = await _accountRepo.FindById(request.ToAccountId);
            if (toAccount is null || toAccount.UserId != userId || !toAccount.IsActive)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = "Destination account not found."
                    }
                };
            }
            if (request.FromAccountId == request.ToAccountId)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_TRANSFER",
                        Message = "Source and destination accounts must be different."
                    }
                };
            }
            if (request.Amount <= 0)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_TRANSFER_AMOUNT",
                        Message = "Transfer amount must be greater than zero."
                    }
                };
            }

            if (fromAccount.CurrentBalance < request.Amount)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INSUFFICIENT_BALANCE",
                        Message = "Insufficient balance for this transfer."
                    }
                };
            }

            var transferId = Guid.NewGuid();
            var outgoingTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountId = fromAccount.Id,
                Amount = request.Amount,
                TransactionType = Domain.Enums.TransactionType.Transfer,
                TransactionDirection = Domain.Enums.TransactionDirection.Out,
                TransferId = transferId,
                Description = request.Description,
                TransactionDate = request.TransactionDate
            };

            var incomingTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountId = toAccount.Id,
                Amount = request.Amount,
                TransactionType = Domain.Enums.TransactionType.Transfer,
                TransactionDirection = Domain.Enums.TransactionDirection.In,
                TransferId = transferId,
                Description = request.Description,
                TransactionDate = request.TransactionDate
            };

            fromAccount.CurrentBalance -= request.Amount;
            fromAccount.UpdatedAt = DateTime.UtcNow;

            toAccount.CurrentBalance += request.Amount;
            toAccount.UpdatedAt = DateTime.UtcNow;

            await _transactionRepo.AddTransaction(outgoingTransaction);
            await _transactionRepo.AddTransaction(incomingTransaction);

            await _accountRepo.UpdateAccount(fromAccount);
            await _accountRepo.UpdateAccount(toAccount);

            var response = new List<TransactionResponse>
            {
                new TransactionResponse
                {
                    Id = outgoingTransaction.Id,
                    AccountId = fromAccount.Id,
                    AccountName = fromAccount.Name,
                    CategoryId = null,
                    CategoryName = null,
                    Amount = outgoingTransaction.Amount,
                    TransactionType = outgoingTransaction.TransactionType,
                    TransactionDirection = outgoingTransaction.TransactionDirection,
                    Merchant = null,
                    Description = outgoingTransaction.Description,
                    TransactionDate = outgoingTransaction.TransactionDate,
                    CreatedAt = outgoingTransaction.CreatedAt,
                    IsActive=true
                },
                new TransactionResponse
                {
                    Id = incomingTransaction.Id,
                    AccountId = toAccount.Id,
                    AccountName = toAccount.Name,
                    CategoryId = null,
                    CategoryName = null,
                    Amount = incomingTransaction.Amount,
                    TransactionType = incomingTransaction.TransactionType,
                    TransactionDirection = incomingTransaction.TransactionDirection,
                    Merchant = null,
                    Description = incomingTransaction.Description,
                    TransactionDate = incomingTransaction.TransactionDate,
                    CreatedAt = incomingTransaction.CreatedAt,
                    IsActive=true
                }
            };

            return new Result<List<TransactionResponse>>
            {
                IsSuccess = true,
                Value = response
            };
        }
    }
}