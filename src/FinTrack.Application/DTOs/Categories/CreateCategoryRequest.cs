using FinTrack.Domain.Enums;

namespace FinTrack.Application.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }//parentcategory
        

    }
}