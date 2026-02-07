using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO
{
    public class BudgetCategoryRequestDTO
    {
        [Required(ErrorMessage = "Budget category name is required!")]
        public string BudgetCategoryName {  get; set; }

        [Required(ErrorMessage = "Budget category description is required!")]
        public string BudgetCategoryDescription { get; set; }

        public int IsActive { get; set; }
    }
}
