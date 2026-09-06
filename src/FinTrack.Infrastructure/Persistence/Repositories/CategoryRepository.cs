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
            await _context.SaveChangesAsync();
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
        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

    }
}