using FinTrack.Application.Common.Results;
using FinTrack.Application.DTOs.Categories;
using FinTrack.Application.Interfaces;
using FinTrack.Domain.Entities;
namespace FinTrack.Application.Features.Categories
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepo, IUnitOfWork unitOfWork)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CategoryResponse>> CreateCategory(CreateCategoryRequest request,Guid userId)
        {
            var category = new Category
            {
                UserId = userId,
                Name = request.Name.Trim(),
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId,
                IsSystemCategory = false,
                IsActive = true
            };

            if (request.ParentCategoryId.HasValue)
            {
                var parentCategory = await _categoryRepo.FindById(request.ParentCategoryId.Value);
                if (parentCategory is null || !parentCategory.IsActive || (!parentCategory.IsSystemCategory && parentCategory.UserId != userId))
                {
                    return new Result<CategoryResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "PARENT_CATEGORY_NOT_FOUND",
                            Message = "Parent category not found."
                        }
                    };
                }
            }

            if (await _categoryRepo.ExistsByName(request.Name, userId))
            {
                return new Result<CategoryResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_ALREADY_EXISTS",
                        Message = "A category with this name already exists."
                    }
                };
            }
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _categoryRepo.AddCategory(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                IsSystemCategory = category.IsSystemCategory,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return new Result<CategoryResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<CategoryResponse>> GetCategory(Guid categoryId,Guid userId)
        {
            var category = await _categoryRepo.FindById(categoryId);

            if (category is null ||(!category.IsSystemCategory && category.UserId != userId))
            {
                return new Result<CategoryResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_NOT_FOUND",
                        Message = "Category not found."
                    }
                };
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                IsSystemCategory = category.IsSystemCategory,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return new Result<CategoryResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<List<CategoryResponse>>> GetCategories(Guid userId)
        {
            var categories = await _categoryRepo.GetByUserId(userId);

            var response = categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                IsSystemCategory = category.IsSystemCategory,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            }).ToList();

            return new Result<List<CategoryResponse>>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result<CategoryResponse>> UpdateCategory(Guid categoryId,UpdateCategoryRequest request,Guid userId)
        {
            var category = await _categoryRepo.FindById(categoryId);

            if (category is null || !category.IsActive ||(!category.IsSystemCategory && category.UserId != userId))
            {
                return new Result<CategoryResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_NOT_FOUND",
                        Message = "Category not found."
                    }
                };
            }
            if (category.IsSystemCategory)
            {
                return new Result<CategoryResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "SYSTEM_CATEGORY_MODIFICATION_NOT_ALLOWED",
                        Message = "System categories cannot be modified."
                    }
                };
            }

            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId.Value == category.Id)
                {
                    return new Result<CategoryResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "INVALID_PARENT_CATEGORY",
                            Message = "A category cannot be its own parent."
                        }
                    };
                }

                var parentCategory = await _categoryRepo.FindById(
                    request.ParentCategoryId.Value);

                if (parentCategory is null || !parentCategory.IsActive || (!parentCategory.IsSystemCategory && parentCategory.UserId != userId))
                {
                    return new Result<CategoryResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "PARENT_CATEGORY_NOT_FOUND",
                            Message = "Parent category not found."
                        }
                    };
                }

                if (await _categoryRepo.HasParentCategory(category.Id,request.ParentCategoryId.Value))
                {
                    return new Result<CategoryResponse>
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Code = "INVALID_PARENT_CATEGORY",
                            Message = "This parent category would create a circular hierarchy."
                        }
                    };
                }
            }
            
            if (await _categoryRepo.ExistsByName(request.Name, userId, category.Id))
            {
                return new Result<CategoryResponse>
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_ALREADY_EXISTS",
                        Message = "A category with this name already exists."
                    }
                };
            }

            category.Name = request.Name.Trim();
            category.Description = request.Description;
            category.ParentCategoryId = request.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _categoryRepo.UpdateCategory(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                IsSystemCategory = category.IsSystemCategory,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return new Result<CategoryResponse>
            {
                IsSuccess = true,
                Value = response
            };
        }
        public async Task<Result> DeactivateCategory(Guid categoryId,Guid userId)
        {
            var category = await _categoryRepo.FindById(categoryId);

            if (category is null || !category.IsActive ||(!category.IsSystemCategory && category.UserId != userId))
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_NOT_FOUND",
                        Message = "Category not found."
                    }
                };
            }
            if (category.IsSystemCategory)
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "SYSTEM_CATEGORY_MODIFICATION_NOT_ALLOWED",
                        Message = "System categories cannot be modified."
                    }
                };
            }

            if (await _categoryRepo.HasActiveChildren(category.Id))
            {
                return new Result
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Code = "CATEGORY_HAS_ACTIVE_CHILDREN",
                        Message = "Category cannot be deactivated while it has active child categories."
                    }
                };
            }

            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _categoryRepo.UpdateCategory(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return new Result
            {
                IsSuccess = true,
            };
        }

    }
}