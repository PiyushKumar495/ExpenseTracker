using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;
namespace FinTrack.Domain.Entities
{
    public class Account:BaseEntity
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public AccountType AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public required string Currency { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }=true;
        public IEnumerable<Transaction> Transactions { get; set; }=[];


    }
}