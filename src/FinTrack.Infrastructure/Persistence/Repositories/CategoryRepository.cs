using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace FinTrack.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly FinTrackDbContext _context;
        public CategoryRepository(FinTrackDbContext context)
        {
            _context=context;
        }
        public async Task<Category> AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            return category;
        }
        public async Task<Category?> FindById(Guid Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c=>c.Id==Id);
        }
        public async Task<List<Category>> GetByUserId(Guid userId)
        {
            return await _context.Categories.Where(c=>c.UserId==userId||c.IsSystemCategory).ToListAsync();
        }

        public async Task<bool> HasParentCategory(Guid categoryId,Guid potentialParentId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == potentialParentId);

            while (category?.ParentCategoryId != null)
            {
                if (category.ParentCategoryId == categoryId)
                {
                    return true;
                }

                category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.ParentCategoryId.Value);
            }

            return false;
        }
        public Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Task.FromResult(category);
        }
        public async Task<bool> HasActiveChildren(Guid categoryId)
        {
            return await _context.Categories
                .AnyAsync(c =>
                    c.ParentCategoryId == categoryId &&
                    c.IsActive);
        }
        public async Task<bool> ExistsByName(string name, Guid userId, Guid? excludeCategoryId = null)
        {
            return await _context.Categories.AnyAsync(c =>
                c.UserId == userId &&
                c.IsActive &&
                c.Name.Trim().ToLower() == name.Trim().ToLower()&&
                (!excludeCategoryId.HasValue || c.Id != excludeCategoryId.Value)
            );
        }

    }
}