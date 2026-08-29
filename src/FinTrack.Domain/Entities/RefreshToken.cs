using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities
{
    public class RefreshToken:BaseEntity
    {
        public Guid UserId { get; set; }
        public required string TokenHash { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public Guid ReplacedByTokenId{ get; set; }

    }

}