using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.Category
{
    public class BudgetCategoryModel
    {
        [Key]
        public int BudgetCategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BudgetCategoryName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string BudgetCategoryDescription { get; set; }

        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
