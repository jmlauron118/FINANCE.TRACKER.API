using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.Category;
using FINANCE.TRACKER.API.Models.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<ActionModel> Actions { get; set; }

        public DbSet<RoleModel> Roles { get; set; }

        public DbSet<ModuleModel> Modules { get; set; }

        public DbSet<UserRoleModel> UserRoles { get; set; }

        public DbSet<ModuleActionModel> ModuleActions { get; set; }

        public DbSet<ModuleAccessModel> ModuleAccesses { get; set; }

        public DbSet<BudgetCategoryModel> BudgetCategories { get; set; }

        public DbSet<ExpensesCategoryModel> ExpensesCategories { get; set; }

        public DbSet<BudgetEntryModel> BudgetEntries { get; set; }

        public DbSet<ExpensesBudgetModel> ExpensesBudget { get; set; }

        public DbSet<ExpensesBudgetCategoryModel> ExpensesBudgetCategories { get; set; }
    }
}
