using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Savings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SavingsController(ISavingsService _savingsService) : Controller
    {
        [HttpGet("get-all-savings-transactions")]
        public async Task<IActionResult> GetAllSavingsTransactions()
        {
            return Ok(new ResponseModel<IEnumerable<SavingsTransactionResponseDTO>>
            {
                Data = await _savingsService.GetAllSavingsTransactions(),
                Message = "Savings transactions has been fetched!"
            });
        }

        [HttpPost("add-savings-transaction")]
        public async Task<IActionResult> AddSavingsTransaction(SavingsTransactionRequestDTO request)
        {
            try
            {
                await _savingsService.AddSavingsTransaction(request);

                return Ok(new ResponseModel<SavingsTransactionResponseDTO>
                {
                    Message = "Transaction created successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-savings-transaction")]
        public async Task<IActionResult> ModifySavingsTransaction(SavingsTransactionModifyDTO request)
        {
            try
            {
                await _savingsService.ModifySavingsTransaction(request);

                return Ok(new ResponseModel<SavingsTransactionResponseDTO>
                {
                    Message = "Transaction modified successfully!"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-savings-transaction")]
        public async Task<IActionResult> RemoveSavingsTransaction(int transactionId)
        {
            try
            {
                await _savingsService.RemoveSavingsTransaction(transactionId);

                return Ok(new ResponseModel<SavingsTransactionResponseDTO>
                {
                    Message = "Transaction has been deleted!"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
