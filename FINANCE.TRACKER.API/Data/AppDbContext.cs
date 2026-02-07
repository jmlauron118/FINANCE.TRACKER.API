using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.Category;
using FINANCE.TRACKER.API.Models.Savings;
using FINANCE.TRACKER.API.Models.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #region User Manager
        public DbSet<UserModel> Users { get; set; }

        public DbSet<ActionModel> Actions { get; set; }

        public DbSet<RoleModel> Roles { get; set; }

        public DbSet<ModuleModel> Modules { get; set; }

        public DbSet<UserRoleModel> UserRoles { get; set; }

        public DbSet<ModuleActionModel> ModuleActions { get; set; }

        public DbSet<ModuleAccessModel> ModuleAccesses { get; set; }
        #endregion User Manager

        #region Budget Manager
        public DbSet<BudgetCategoryModel> BudgetCategories { get; set; }

        public DbSet<ExpensesCategoryModel> ExpensesCategories { get; set; }

        public DbSet<BudgetEntryModel> BudgetEntries { get; set; }

        public DbSet<ExpensesBudgetModel> ExpensesBudget { get; set; }

        public DbSet<ExpensesBudgetCategoryModel> ExpensesBudgetCategories { get; set; }
        #endregion Budget Manager

        #region Savings and Investments
        public DbSet<SavingsTransactionModel> SavingsTransactions { get; set; }

        public DbSet<InvestmentModel> Investments { get; set; }

        public DbSet<SavingsTransactionTypeModel> SavingsTransactionTypes { get; set; }

        public DbSet<InvestmentTypeModel> InvestmentTypes { get; set; }
        #endregion Savings and Investments
    }
}
