using FinTrack.Domain.Entities;
namespace FinTrack.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task <Category>AddCategory(Category category);
        Task <Category?> FindById(Guid Id);
        Task <List<Category>> GetByUserId(Guid userId);
        Task <Category> UpdateCategory(Category category);
        Task<bool> HasParentCategory(Guid categoryId, Guid potentialParentId);

    }
}