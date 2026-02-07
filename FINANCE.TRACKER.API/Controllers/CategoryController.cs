using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO;
using FINANCE.TRACKER.API.Models.DTO.Category.ExpenseCategoryDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentTypeDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionTypeDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController
        (
            IBudgetCategoryService _budgetCategoryService,
            IExpenseCategoryService _expenseCategoryService,
            ISavingsTransactionTypeService _transactionTypeService,
            IInvestmentTypeService _investmentTypeService
        ) : Controller
    {
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

        #region Savings Transaction Type
        [HttpGet("get-all-savings-transaction-types")]
        public async Task<IActionResult> GetAllSavingsTransactionTypes(int status = 2, int type = 2)
        {
            return Ok(new ResponseModel<IEnumerable<SavingsTransactionTypeResponseDTO>>
            {
                Data = await _transactionTypeService.GetAllSavingsTransactionTypes(status, type),
                Message = "Savings transaction types fetched successfully!"
            });
        }

        [HttpPost("add-savings-transaction-type")]
        public async Task<IActionResult> AddSavingsTransactionType(SavingsTransactionTypeRequestDTO request)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<SavingsTransactionTypeResponseDTO>
                {
                    Data = await _transactionTypeService.AddSavingsTransactionType(request),
                    Message = "Savings transaction type added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-savings-transaction-type")]
        public async Task<IActionResult> ModifySavingsTransactionType(SavingsTransactionTypeModifyDTO request)
        {
            try
            {
                return Ok(new ResponseModel<SavingsTransactionTypeResponseDTO>
                {
                    Data = await _transactionTypeService.ModifySavingsTransactionType(request),
                    Message = "Savings transaction type modified successfully!!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Savings Transaction Type

        #region Investment Type
        [HttpGet("get-all-investment-types")]
        public async Task<IActionResult> GetAllInvestmentTypes(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<InvestmentTypeResponseDTO>>
            {
                Data = await _investmentTypeService.GetAllInvestmentTypes(status),
                Message = "Investment types fetched successfully!"
            });
        }

        [HttpPost("add-investment-type")]
        public async Task<IActionResult> AddInvestmentType(InvestmentTypeRequestDTO request)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<InvestmentTypeResponseDTO>
                {
                    Data = await _investmentTypeService.AddInvestmentType(request),
                    Message = "Investment type added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-investment-type")]
        public async Task<IActionResult> ModifyInvestmentType(InvestmentTypeModifyDTO request)
        {
            try
            {
                return Ok(new ResponseModel<InvestmentTypeResponseDTO>
                {
                    Data = await _investmentTypeService.ModifyInvestmentType(request),
                    Message = "Investment type modified successfully!!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Investment Type
    }
}
