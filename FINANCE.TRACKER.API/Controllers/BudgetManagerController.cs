using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.ExpensesBudget;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetManagerController(IBudgetEntryService _budgetManagerService, IExpensesBudgetService _expensesBudgetService) : Controller
    {
        #region Budget Entry
        [HttpGet("get-budget-entires")]
        public async Task<IActionResult> GetAllBudgetEntires([FromQuery] BudgetEntryParameters budgetEntryParameters)
        {
            var budgetEntries = await _budgetManagerService.GetAllBudgetEntries(budgetEntryParameters);
            var metadata = new
            {
                totalCount = budgetEntries.TotalCount,
                pageSize = budgetEntries.PageSize,
                currentPage = budgetEntries.CurrentPage,
                totalPages = budgetEntries.TotalPages,
                hasNext = budgetEntries.HasNext,
                hasPrevious = budgetEntries.HasPrevious,
                totalIncome = budgetEntries.TotalIncome,
                totalSavings = budgetEntries.TotalSavings,
                totalExpenses = budgetEntries.TotalExpenses,
                totalBalance = budgetEntries.TotalBalance
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(new ResponseModel<IEnumerable<BudgetEntryResponseDTO>>
            {
                Data = await _budgetManagerService.GetAllBudgetEntries(budgetEntryParameters),
                Message = "All budget entries fetched successfully!"
            });
        }

        [HttpPost("add-budget-entry")]
        public async Task<IActionResult> AddBudgetEntry([FromBody] BudgetEntryRequestDTO budgetRequest)
        {
            return Created(string.Empty, new ResponseModel<BudgetEntryResponseDTO>
            {
                Data = await _budgetManagerService.AddBudgetEntry(budgetRequest),
                Message = "Budget entry added successfully!"
            });
        }

        [HttpPost("sync-budget-entries")]
        public async Task<IActionResult> SyncBudgetEntries([FromBody] List<BudgetEntryRequestDTO> budgetRequestList)
        {
            try
            {
                await _budgetManagerService.SyncBudgetEntries(budgetRequestList);

                return Created(string.Empty, new ResponseModel<object>
                {
                    Message = "Unbudgeted expenses successfully synced!"
                });
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("modify-budget-entry")]
        public async Task<IActionResult> ModifyBudgetEntry([FromBody] BudgetEntryModifyDTO budgetRequest)
        {
            try
            {
                return Ok(new ResponseModel<BudgetEntryResponseDTO>
                {
                    Data = await _budgetManagerService.ModifyBudgetEntry(budgetRequest),
                    Message = "Budget entry modified successfully!"
                });
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-budget-entry")]
        public async Task<IActionResult> RemoveBudgetEntry(int id)
        {
            try
            {
                await _budgetManagerService.RemoveBudgetEntry(id);

                return Ok(new ResponseModel<object>
                {
                    Message = "Budget entry has been removed!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-budget-entry-bulk")]
        public async Task<IActionResult> RemoveBudgetEntryBulk([FromBody] List<BudgetEntryDeleteDTO> idList)
        {
            try
            {
                await _budgetManagerService.RemoveBudgetEntryBulk(idList);

                return Ok(new ResponseModel<object>
                {
                    Message = "Budget entries has been removed!"
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion Budget Entry

        #region Expenses Budget
        [HttpGet("get-expenses-budget-by-category")]
        public async Task<IActionResult> GetExpensesBudgetByCategory(int categoryId)
        {
            return Ok(new ResponseModel<IEnumerable<ExpensesBudgetResponseDTO>>
            {
                Data = await _expensesBudgetService.GetExpensesBudgetByCategory(categoryId),
                Message = "All expense budget fetched successfully!"
            });
        }

        [HttpPost("add-expenses-budget-bulk")]
        public async Task<IActionResult> AddExpensesBudgetBulk([FromBody] List<ExpenseBudgetRequestDTO> expenseBudgetRequests, int categoryId)
        {
            try
            {
                await _expensesBudgetService.AddExpensesBudgetBulk(expenseBudgetRequests, categoryId);

                return Created(string.Empty, new ResponseModel<object>
                {
                    Message = GetMessage(categoryId)
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion Expenses Budget

        private string GetMessage(int category) =>
            category switch
            {
                1 => "Budgeted expenses successfully updated!",
                2 => "Unbudgeted expenses (Monthly) successfully updated!",
                3 => "Unbudgeted expenses (Payroll) successfully updated!",
                _ => "Expenses budget successfully updated!"
            };
    }
}
