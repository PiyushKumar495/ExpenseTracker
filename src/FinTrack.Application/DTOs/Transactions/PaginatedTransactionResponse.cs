using FinTrack.Application.DTOs.Common;

namespace FinTrack.Application.DTOs.Transactions
{
    public class PaginatedTransactionResponse
    {
        public List<TransactionResponse> Transactions { get; set; } = new();
        public PaginationResponse Pagination { get; set; } = new();
    }
}