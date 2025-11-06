using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO;
using FINANCE.TRACKER.API.Models.DTO.Category.ExpenseCategoryDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IBudgetCategoryService _budgetCategoryService;
        private readonly IExpenseCategoryService _expenseCategoryService;

        public CategoryController(IBudgetCategoryService budgetCategoryService,
                                    IExpenseCategoryService expenseCategoryService)
        {
            _budgetCategoryService = budgetCategoryService;
            _expenseCategoryService = expenseCategoryService;
        }

        #region Budget Category
        [HttpGet("get-all-budget-categories")]
        public async Task<IActionResult> GetAllBudgetCategories(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<BudgetCategoryResponseDTO>>
            {
                Data = await _budgetCategoryService.GetAllBudgetCategories(status),
                Message = "Budget categories fetched successfully!"
            });
        }

        [HttpPost("add-budget-category")]
        public async Task<IActionResult> AddBudgetCategory([FromBody] BudgetCategoryRequestDTO budgetCategory)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<BudgetCategoryResponseDTO>
                {
                    Data = await _budgetCategoryService.AddBudgetCategory(budgetCategory),
                    Message = "Budget category added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-budget-category")]
        public async Task<IActionResult> ModifyBudgetCategory([FromBody] BudgetCategoryModifyDTO budgetCategory)
        {
            try
            {
                return Ok(new ResponseModel<BudgetCategoryResponseDTO>
                {
                    Data = await _budgetCategoryService.ModifyBudgetCategory(budgetCategory),
                    Message = "Budget category modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Budget Category

        #region Expense Category
        [HttpGet("get-all-expense-categories")]
        public async Task<IActionResult> GetAllExpenseCategories(int status = 2)
        {
            try
            {
                return Ok(new ResponseModel<IEnumerable<ExpenseCategoryResponseDTO>>
                {
                    Data = await _expenseCategoryService.GetAllExpenseCategories(status),
                    Message = "Expense categories fetched successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-expense-category")]
        public async Task<IActionResult> AddExpenseCategory([FromBody] ExpenseCategoryRequestDTO expenseCategory)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<ExpenseCategoryResponseDTO>
                {
                    Data = await _expenseCategoryService.AddExpenseCategory(expenseCategory),
                    Message = "Expense category added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-expense-category")]
        public async Task<IActionResult> ModifyExpenseCategory([FromBody] ExpenseCategoryModifyDTO expenseCategory)
        {
            try
            {
                return Ok(new ResponseModel<ExpenseCategoryResponseDTO>
                {
                    Data = await _expenseCategoryService.ModifyExpenseCategory(expenseCategory),
                    Message = "Expense category modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Expense Category
    }
}
