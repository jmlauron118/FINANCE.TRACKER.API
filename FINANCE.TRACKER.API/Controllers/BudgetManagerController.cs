using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetManagerController : Controller
    {
        private readonly IBudgetEntryService _budgetManagerService;

        public BudgetManagerController(IBudgetEntryService budgetManagerService)
        {
            _budgetManagerService = budgetManagerService;
        }

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
                hasPrevious = budgetEntries.HasPrevious
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

        [HttpPost("add-budget-entry-bulk")]
        public async Task<IActionResult> AddBudgetEntryBulk([FromBody] List<BudgetEntryRequestDTO> budgetRequestList)
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
    }
}
