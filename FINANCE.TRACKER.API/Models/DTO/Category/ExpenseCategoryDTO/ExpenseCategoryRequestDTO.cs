using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.Category.ExpenseCategoryDTO
{
    public class ExpenseCategoryRequestDTO
    {

        [Required(ErrorMessage = "Expense category name is required!")]
        public string? ExpenseCategoryName { get; set; }

        [Required(ErrorMessage = "Expense category description is required!")]
        public string? ExpenseCategoryDescription { get; set; }
        public int IsActive { get; set; }
    }
}
