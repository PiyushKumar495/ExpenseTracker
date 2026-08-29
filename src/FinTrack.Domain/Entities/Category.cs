using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities
{
    public class Category:BaseEntity
    {
        public Guid? UserId { get; set; }//Owner; null for system categories
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }//parentcategory
        public bool IsSystemCategory { get; set; }
        public bool IsActive { get; set; }=true;
        // Self-referencing category relationship
        public Category? ParentCategory { get; set; }
        public ICollection<Category> ChildCategories { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; }=[];

    }
}

/*
we arehaving userid so that if user create any category then we can have their id store too
*/