using FinTrack.Domain.Enums;

namespace FinTrack.Application.DTOs.Transactions
{
    public class TransactionFilterRequest
    {
        public Guid? AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public TransactionDirection? TransactionDirection { get; set; }

        public TransactionType? TransactionType { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? IsActive { get; set; }



    }
}

