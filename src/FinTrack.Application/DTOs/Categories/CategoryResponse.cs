namespace FinTrack.Application.DTOs.Categories
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }//parentcategory
        public bool IsSystemCategory { get; set; }
        public bool IsActive { get; set; }
        

    }
}