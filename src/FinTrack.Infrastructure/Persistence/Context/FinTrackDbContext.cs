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
            
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    UserId = null,
                    Name = "Food",
                    Description = "Food and dining expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    UserId = null,
                    Name = "Transportation",
                    Description = "Transportation and travel expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    UserId = null,
                    Name = "Shopping",
                    Description = "Shopping and purchases",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                    UserId = null,
                    Name = "Bills",
                    Description = "Utility and household bills",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                    UserId = null,
                    Name = "Entertainment",
                    Description = "Entertainment and leisure expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                    UserId = null,
                    Name = "Health",
                    Description = "Healthcare and medical expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                    UserId = null,
                    Name = "Education",
                    Description = "Education and learning expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                },
                new Category
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                    UserId = null,
                    Name = "Subscriptions",
                    Description = "Recurring subscription expenses",
                    ParentCategoryId = null,
                    IsSystemCategory = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 6)
                }
            );
            
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
        public DbSet<Account>Accounts{get;set;}
        public DbSet<Category> Categories { get; set; }
    }
}