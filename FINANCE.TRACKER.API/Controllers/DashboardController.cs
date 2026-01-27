using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.Dashboard;
using FINANCE.TRACKER.API.Services.Interfaces.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("get-summary")]
        public async Task<IActionResult> GetSummmary()
        {
            return Ok(new ResponseModel<SummaryResponseDTO>
            {
                Data = await _dashboardService.GetSummary(),
                Message = "Summary fetched successfully."
            });
        }


        [HttpGet("get-recent-transactions")]
        public async Task<IActionResult> GetRecentTransactions()
        {
            return Ok(new ResponseModel<IEnumerable<RecentTransactionsResponseDTO>>
            {
                Data = await _dashboardService.GetRecentTransactions(),
                Message = "Recent transactions fetched successfully."
            });
        }

        [HttpGet("get-ytd-income")]
        public async Task<IActionResult> GetYTDIncome()
        {
            return Ok(new ResponseModel<IEnumerable<YTDIncomeResponseDTO>>
            {
                Data = await _dashboardService.GetYTDIncome(),
                Message = "YTD Income fetched successfully."
            });
        }

        [HttpGet("get-monthly-budget")]
        public async Task<IActionResult> GetMonthlyBudget()
        {
            return Ok(new ResponseModel<MonthlyBudgetResponseDTO>
            {
                Data = await _dashboardService.GetMonthlyBudget(),
                Message = "Monthly budget fetched successfully."
            });
        }

        [HttpGet("get-expenses-by-category")]
        public async Task<IActionResult> GetExpensesByCategory()
        {
            return Ok(new ResponseModel<IEnumerable<ExpensesByCategoryDTO>>
            {
                Data = await _dashboardService.GetExpensesByCategory(),
                Message = "Expenses by category fetched successfully."
            });
        }

        [HttpGet("get-activity")]
        public async Task<IActionResult> GetActivity()
        {
            return Ok(new ResponseModel<IEnumerable<ActivityResponseDTO>>
            {
                Data = await _dashboardService.GetActivity(),
                Message = "Activity fetched successfully."
            });
        }
    }
}
