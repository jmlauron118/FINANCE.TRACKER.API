using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.BudgetManager
{
    public class ExpensesBudgetCategoryModel
    {
        [Key]
        public int ExpensesBudgetCategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Expenses Budget Category Name is required.")]
        public string? ExpensesBudgetCategoryName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [Required(ErrorMessage = "Expenses Budget Category is required.")]
        public string? Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
