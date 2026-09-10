using FinTrack.Application.DTOs.Common;
using FinTrack.Application.DTOs.Transactions;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinTrackDbContext _context;

        public TransactionRepository(FinTrackDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> FindById(Guid id)
        {
            return await _context.Transactions
                .Include(t=>t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(List<Transaction>Transactions,int TotalCount)> GetByUserId(Guid userId,TransactionFilterRequest request,PaginationRequest pagination)
        {
            var query= _context.Transactions
                .Include(t=>t.Account)
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            if (request.AccountId.HasValue)
            {
                query = query.Where(t => t.AccountId == request.AccountId.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == request.CategoryId.Value);
            }

            if (request.TransactionDirection.HasValue)
            {
                query = query.Where(t =>
                    t.TransactionDirection == request.TransactionDirection.Value);
            }

            if (request.TransactionType.HasValue)
            {
                query = query.Where(t =>
                    t.TransactionType == request.TransactionType.Value);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(t =>
                    t.TransactionDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(t =>
                    t.TransactionDate <= request.ToDate.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(t =>
                    t.IsActive == request.IsActive.Value);
            }

            var totalCount=await query.CountAsync();

            if (pagination.SortBy.Equals("TransactionDate", StringComparison.OrdinalIgnoreCase))
            {
                query = pagination.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(t => t.TransactionDate).ThenBy(t=>t.Id)
                    : query.OrderByDescending(t => t.TransactionDate).ThenBy(t=>t.Id);
            }
            else if (pagination.SortBy.Equals("Amount", StringComparison.OrdinalIgnoreCase))
            {
                query = pagination.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(t => t.Amount).ThenBy(t=>t.Id)
                    : query.OrderByDescending(t => t.Amount).ThenBy(t=>t.Id);
            }
            else if (pagination.SortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
            {
                query = pagination.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(t => t.CreatedAt).ThenBy(t=>t.Id)
                    : query.OrderByDescending(t => t.CreatedAt).ThenBy(t=>t.Id);
            }
            else if (pagination.SortBy.Equals("Merchant", StringComparison.OrdinalIgnoreCase))
            {
                query = pagination.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(t => t.Merchant).ThenBy(t=>t.Id)
                    : query.OrderByDescending(t => t.Merchant).ThenBy(t=>t.Id);
            }

            var transactions = await query
                .Skip((pagination.Pagenumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (transactions, totalCount);
            
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<Transaction>> GetByTransferId(Guid transferId)
        {
            return await _context.Transactions
                .Where(t => t.TransferId == transferId)
                .ToListAsync();
        }
    }
}