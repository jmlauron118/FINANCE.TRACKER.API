using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetManagerController : Controller
    {
        private readonly IBudgetManagerService _budgetManagerService;

        public BudgetManagerController(IBudgetManagerService budgetManagerService)
        {
            _budgetManagerService = budgetManagerService;
        }

        [HttpGet("get-all-budget-entires")]
        public async Task<IActionResult> GetAllBudgetEntires()
        {
            return Ok(new ResponseModel<IEnumerable<BudgetManagerResponseDTO>>
            {
                Data = await _budgetManagerService.GetAllBudgetEntries(),
                Message = "All budget entries fetched successfully!"
            });
        }

        [HttpPost("add-budget-entry")]
        public async Task<IActionResult> AddBudgetEntry([FromBody] BudgetManagerRequestDTO budgetRequest)
        {
            return Created(string.Empty, new ResponseModel<BudgetManagerResponseDTO>
            {
                Data = await _budgetManagerService.AddBudgetEntry(budgetRequest),
                Message = "Budget entry added successfully!"
            });
        }

        [HttpPost("add-budget-entry-bulk")]
        public async Task<IActionResult> AddBudgetEntryBulk([FromBody] List<BudgetManagerRequestDTO> budgetRequestList)
        {
            try
            {
                await _budgetManagerService.AddBudgetEntryBulk(budgetRequestList);

                return Created(string.Empty, new ResponseModel<object>
                {
                    Message = "Budget entries added successfully!"
                });
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("modify-budget-entry")]
        public async Task<IActionResult> ModifyBudgetEntry([FromBody] BudgetManagerModifyDTO budgetRequest)
        {
            try
            {
                return Ok(new ResponseModel<BudgetManagerResponseDTO>
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

        [HttpDelete("delete-budget-entry")]
        public async Task<IActionResult> RemoveBudgetEntry([FromBody] List<BudgetManagerDeleteDTO> idList)
        {
            try
            {
                await _budgetManagerService.RemoveBudgetEntry(idList);

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
    }
}
