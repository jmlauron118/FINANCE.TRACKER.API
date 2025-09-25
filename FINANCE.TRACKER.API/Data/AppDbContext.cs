using FINANCE.TRACKER.API.Model.Category;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BudgetCategoryModel> BudgetCategories { get; set; }
        public DbSet<ExpensesCategoryModel> ExpensesCategories { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
