using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Context
{
    public class FinTrackDbContext : DbContext
    {
        public FinTrackDbContext(DbContextOptions<FinTrackDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction>()
                        .HasOne<User>()
                        .WithMany(u => u.Transactions)
                        .HasForeignKey(t => t.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
                        .HasOne<Account>()
                        .WithMany(a => a.Transactions)
                        .HasForeignKey(t => t.AccountId)
                        .OnDelete(DeleteBehavior.NoAction);
                        
            modelBuilder.Entity<Transaction>()
                        .HasOne<Category>()
                        .WithMany(c => c.Transactions)
                        .HasForeignKey(t => t.CategoryId)
                        .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}