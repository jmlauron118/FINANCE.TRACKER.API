using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Repositories
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        //Totals
        public decimal TotalIncome { get; private set; }
        public decimal TotalSavings { get; private set; }
        public decimal TotalExpenses { get; private set; }
        public decimal TotalBalance => TotalIncome - (TotalSavings + TotalExpenses);

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize, decimal totalIncome, decimal totalSavings, decimal totalExpenses)
            : this(items, count, pageNumber, pageSize)
        {
            TotalIncome = totalIncome;
            TotalSavings = totalSavings;
            TotalExpenses = totalExpenses;
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            if (typeof(T) == typeof(BudgetEntryResponseDTO))
            {
                var typeSource = source as IQueryable<BudgetEntryResponseDTO>;

                if(typeSource != null)
                {
                    var totalIncome = await typeSource
                        .Where(be => be.BudgetCategoryName != null && be.BudgetCategoryName.ToLower() == "income")
                        .SumAsync(be => be.Amount ?? 0m);

                    var totalSavings = await typeSource
                        .Where(be => be.BudgetCategoryName != null && be.BudgetCategoryName.ToLower() == "savings")
                        .SumAsync(be => be.Amount ?? 0m);

                    var totalExpenses = await typeSource
                        .Where(be => be.BudgetCategoryName != null && be.BudgetCategoryName.ToLower().Contains("expenses"))
                        .SumAsync(be => be.Amount ?? 0m);

                    return new PagedList<T>(items, count, pageNumber, pageSize, totalIncome, totalSavings, totalExpenses);
                }
            }

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
