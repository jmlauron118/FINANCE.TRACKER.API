using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Model.Category
{
    public class ExpensesCategoryModel
    {
        [Key]
        public int ExpensesCategoryId { get; set; }

        [Column(TypeName= "nvarchar(50)")]
        public string? ExpensesCategoryName { get; set; }
        public string? ExpensesCategoryDescription { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
