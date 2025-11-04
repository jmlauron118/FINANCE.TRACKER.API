namespace FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO
{
    public class BudgetCategoryRequestDTO
    {
        public string? BudgetCategoryName {  get; set; }
        public string? BudgetCategoryDescription { get; set; }
        public int IsActive { get; set; }
    }
}
