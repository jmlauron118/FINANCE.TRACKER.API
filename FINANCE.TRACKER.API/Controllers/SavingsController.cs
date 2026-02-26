using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Savings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SavingsController(ISavingsService _savingsService, IInvestmentService _investmentService) : Controller
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

        [HttpGet("get-savings-summary")]
        public async Task<IActionResult> GetSavingsSummary()
        {
            return Ok(new ResponseModel<SavingsSummaryResponseDTO>
            {
                Data = await _savingsService.GetSavingsSummary(),
                Message = "Savings summary fetched successfully!"
            });
        }

        [HttpGet("get-all-investments")]
        public async Task<IActionResult> GetAllInvestments()
        {
            return Ok(new ResponseModel<IEnumerable<InvestmentResponseDTO>>
            {
                Data = await _investmentService.GetAllInvestments(),
                Message = "Investment data fetched successfully!"
            });
        }

        [HttpPost("return-from-investment")]
        public async Task<IActionResult> ReturnFromInvestment(ReturnFromInvestmentDTO request)
        {
            try
            {
                await _investmentService.ReturnFromInvestment(request);

                return Ok(new ResponseModel<InvestmentResponseDTO>
                {
                    Message = "Return from investment processed successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("get-return-savings-transaction")]
        public async Task<IActionResult> GetReturnSavingsTransaction(int returnTransactionId)
        {
            try
            {
                var returnTransaction = await _investmentService.GetReturnSavingsTransaction(returnTransactionId);

                if (returnTransaction == null) return NotFound(new { message = "Return transaction not found." });

                return Ok(new ResponseModel<ReturnTransactionResponseDTO>
                {
                    Data = returnTransaction,
                    Message = "Return transaction fetched successfully!"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
