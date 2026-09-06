using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Categories;

namespace FinTrack.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> CreateCategory(CreateCategoryRequest request,Guid userId);
        Task<Result<CategoryResponse>> GetCategory(Guid categoryId,Guid userId);
        Task<Result<List<CategoryResponse>>> GetCategories(Guid userId);
        Task<Result<CategoryResponse>> UpdateCategory(Guid categoryId,UpdateCategoryRequest request,Guid userId);
        Task<Result> DeactivateCategory(Guid categoryId,Guid userId);

    }
}