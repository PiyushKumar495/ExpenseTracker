using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Common;
using FinTrack.Application.DTOs.Transactions;
using FinTrack.Domain.Entities;
using FinTrack.Domain.Enums;

namespace FinTrack.Application.Interfaces
{
    public class TransactionService:ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(ITransactionRepository transactionRepo,IAccountRepository accountRepo,ICategoryRepository categoryRepo, IUnitOfWork unitOfWork)
        {
            _transactionRepo=transactionRepo;
            _accountRepo=accountRepo;
            _categoryRepo=categoryRepo;
            _unitOfWork=unitOfWork;
        }
        public async Task<Result<TransactionResponse>> CreateTransaction(CreateTransactionRequest request,Guid userId)
        {
            var account = await _accountRepo.FindById(request.AccountId);

            if (account is null || account.UserId != userId || !account.IsActive)
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

                if (category is null || !category.IsActive || (!category.IsSystemCategory && category.UserId != userId))
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

            if (request.TransactionDirection == TransactionDirection.Out && account.CurrentBalance < request.Amount)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INSUFFICIENT_BALANCE",
                        Message = "Insufficient balance for this transaction."
                    }
                };
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

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _transactionRepo.AddTransaction(transaction);
                await _accountRepo.UpdateAccount(account);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                TransferId = transaction.TransferId,
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
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                IsActive=transaction.IsActive
                
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
            
            var response = new TransactionResponse
            {
                AccountId = transaction.AccountId,
                AccountName = transaction.Account!.Name,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category?.Name,
                Amount = transaction.Amount,
                TransferId=transaction.TransferId,
                TransactionType = transaction.TransactionType,
                TransactionDirection = transaction.TransactionDirection,
                Merchant = transaction.Merchant,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                IsActive=transaction.IsActive
            };

            return new Result<TransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }

        public async Task<Result<PaginatedTransactionResponse>> GetTransactions(Guid userId, TransactionFilterRequest request,PaginationRequest pagination)
        {
            var result = await _transactionRepo.GetByUserId(userId,request,pagination);

            var transactions = result.Transactions;

            var response = new PaginatedTransactionResponse
            {
                Transactions = transactions.Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    AccountName = t.Account!.Name,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category?.Name,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    TransactionDirection = t.TransactionDirection,
                    TransferId = t.TransferId,
                    Merchant = t.Merchant,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    IsActive = t.IsActive
                }).ToList(),

                Pagination = new PaginationResponse
                {
                    Pagenumber = pagination.Pagenumber,
                    PageSize = pagination.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = (int)Math.Ceiling(
                        result.TotalCount / (double)pagination.PageSize)
                }
            };

            return new Result<PaginatedTransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<TransactionResponse>> UpdateTransaction(Guid transactionId,UpdateTransactionRequest request,Guid userId)
        {
            var transaction = await _transactionRepo.FindById(transactionId);

            if (transaction is null ||
                transaction.UserId != userId ||
                !transaction.IsActive)
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

            if (transaction.TransferId.HasValue)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "TRANSFER_MODIFICATION_NOT_ALLOWED",
                        Message = "Transfer transactions cannot be modified individually."
                    }
                };
            }

            // Get old account
            var account = await _accountRepo.FindById(transaction.AccountId);

            if (account is null ||
                account.UserId != userId ||
                !account.IsActive)
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

            // Get target account
            var targetAccount = account;

            if (request.AccountId != transaction.AccountId)
            {
                targetAccount = await _accountRepo.FindById(request.AccountId);

                if (targetAccount is null ||
                    targetAccount.UserId != userId ||
                    !targetAccount.IsActive)
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

            // Validate category
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepo.FindById(request.CategoryId.Value);

                if (category is null ||
                    !category.IsActive ||
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

            // Calculate target balance without modifying the database yet

            decimal newTargetBalance = targetAccount.CurrentBalance;

            // If changing accounts, first reverse the old transaction
            if (targetAccount.Id == account.Id)
            {
                if (transaction.TransactionDirection == TransactionDirection.In)
                {
                    newTargetBalance -= transaction.Amount;
                }
                else
                {
                    newTargetBalance += transaction.Amount;
                }
            }

            // Check whether the new transaction can be applied
            if (request.TransactionDirection == TransactionDirection.Out &&
                newTargetBalance < request.Amount)
            {
                return new Result<TransactionResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INSUFFICIENT_BALANCE",
                        Message = "Insufficient balance for this transaction."
                    }
                };
            }

            // Start DB transaction only after all validation succeeds
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Reverse old transaction from old account
                if (transaction.TransactionDirection == TransactionDirection.In)
                {
                    account.CurrentBalance -= transaction.Amount;
                }
                else
                {
                    account.CurrentBalance += transaction.Amount;
                }

                // Apply new transaction to target account
                if (request.TransactionDirection == TransactionDirection.In)
                {
                    targetAccount.CurrentBalance += request.Amount;
                }
                else
                {
                    targetAccount.CurrentBalance -= request.Amount;
                }

                // Update transaction
                transaction.AccountId = request.AccountId;
                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.TransactionDirection = request.TransactionDirection;
                transaction.TransactionType =
                    request.TransactionDirection == TransactionDirection.In
                        ? TransactionType.Income
                        : TransactionType.Expense;
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
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
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
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                IsActive = transaction.IsActive,
                TransferId = transaction.TransferId
            };

            return new Result<TransactionResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }

        public async Task<Result> DeactivateTransaction(Guid transactionId, Guid userId)
        {
            var transaction = await _transactionRepo.FindById(transactionId);

            if (transaction is null || transaction.UserId != userId || !transaction.IsActive)
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

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Normal transaction
                if (transaction.TransferId is null)
                {
                    var account = await _accountRepo.FindById(transaction.AccountId);

                    if (account is null || account.UserId != userId)
                    {
                        await _unitOfWork.RollbackTransactionAsync();

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

                    // Reverse the transaction's effect on the balance
                    if (transaction.TransactionDirection == TransactionDirection.Out)
                    {
                        account.CurrentBalance += transaction.Amount;
                    }
                    else
                    {
                        account.CurrentBalance -= transaction.Amount;
                    }

                    transaction.IsActive = false;
                    transaction.UpdatedAt = DateTime.UtcNow;

                    account.UpdatedAt = DateTime.UtcNow;

                    await _transactionRepo.UpdateTransaction(transaction);
                    await _accountRepo.UpdateAccount(account);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    return new Result
                    {
                        IsSuccess = true
                    };
                }

                // Transfer transaction
                var transferTransactions = await _transactionRepo.GetByTransferId(
                    transaction.TransferId.Value
                );

                foreach (var transferTransaction in transferTransactions)
                {
                    if (!transferTransaction.IsActive)
                        continue;

                    var account = await _accountRepo.FindById(transferTransaction.AccountId);

                    if (account is null || account.UserId != userId)
                    {
                        await _unitOfWork.RollbackTransactionAsync();

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

                    // Reverse the transfer effect
                    if (transferTransaction.TransactionDirection == TransactionDirection.Out)
                    {
                        account.CurrentBalance += transferTransaction.Amount;
                    }
                    else
                    {
                        account.CurrentBalance -= transferTransaction.Amount;
                    }

                    transferTransaction.IsActive = false;
                    transferTransaction.UpdatedAt = DateTime.UtcNow;

                    account.UpdatedAt = DateTime.UtcNow;

                    await _transactionRepo.UpdateTransaction(transferTransaction);
                    await _accountRepo.UpdateAccount(account);
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new Result
                {
                    IsSuccess = true
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
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

            if(fromAccount.Currency!=toAccount.Currency)
            {
                return new Result<List<TransactionResponse>>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "INVALID_TRANSFER_CURRENCY",
                        Message = "Transfers are only allowed between accounts with the same currency."
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

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _transactionRepo.AddTransaction(outgoingTransaction);
                await _transactionRepo.AddTransaction(incomingTransaction);

                await _accountRepo.UpdateAccount(fromAccount);
                await _accountRepo.UpdateAccount(toAccount);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

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
                    IsActive=true,
                    UpdatedAt = outgoingTransaction.UpdatedAt,
                    TransferId=transferId
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
                    IsActive=true,
                    UpdatedAt = incomingTransaction.UpdatedAt,
                    TransferId=transferId
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