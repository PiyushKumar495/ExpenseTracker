using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities
{
    public class User:BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Currency { get; set; }
        public required string TimeZone { get; set; }
        public bool IsActive { get; set; }=true;

        public ICollection<Account> Accounts { get; set; }=[];
        public ICollection<Transaction> Transactions { get; set; }=[];
        public ICollection<Category> Categories { get; set; }=[];
    }

}