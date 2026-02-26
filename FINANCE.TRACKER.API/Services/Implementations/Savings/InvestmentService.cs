using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;
using FINANCE.TRACKER.API.Models.Savings;
using FINANCE.TRACKER.API.Services.Interfaces.Savings;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Savings
{
    public class InvestmentService : IInvestmentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public InvestmentService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        }

        public enum TransactionType
        {
            ReturnFromInvestment = 3
        }

        public async Task<IEnumerable<InvestmentResponseDTO>> GetAllInvestments()
        {
            var investmentData = await (from i in _context.Investments
                                        join it in _context.InvestmentTypes on i.InvestmentTypeId equals it.InvestmentTypeId
                                        join st in _context.SavingsTransactions on i.TransactionId equals st.TransactionId
                                        join rfi in _context.SavingsTransactions on i.ReturnTransactionId equals rfi.TransactionId into rfiGroup
                                        from rfi in rfiGroup.DefaultIfEmpty()
                                        where i.CreatedBy == _userId
                                        select new InvestmentResponseDTO
                                        {
                                            InvestmentId = i.InvestmentId,
                                            InvestmentDate = st.DateUsed,
                                            InvestmentTypeName = it.InvestmentTypeName,
                                            Description = st.Description,
                                            InvestmentAmount = st.Amount,
                                            ReturnTransactionId = i.ReturnTransactionId ?? 0,
                                            RealizedAmount = rfi != null ? i.RelializedAmount : 0,
                                            ReturnDate = rfi != null ? rfi.DateUsed : null
                                        }).ToListAsync();

            return investmentData;
        }

        public async Task ReturnFromInvestment(ReturnFromInvestmentDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var investment = await _context.Investments.FirstOrDefaultAsync(i => i.InvestmentId == request.InvestmentId && i.CreatedBy == _userId);

                if (investment == null) throw new InvalidOperationException("Investment not found.");

                var returnTransaction = new SavingsTransactionModel
                {
                    TransactionTypeId = (int)TransactionType.ReturnFromInvestment,
                    Description = request.ReturnDescription,
                    Amount = request.AmountReturned,
                    Status = 1,
                    DateUsed = request.DateReturned,
                    CreatedBy = _userId,
                    DateCreated = DateTime.Now
                };

                await _context.SavingsTransactions.AddAsync(returnTransaction);
                await _context.SaveChangesAsync();

                var savingsTranData = await _context.SavingsTransactions.FindAsync(investment.TransactionId);

                if (savingsTranData == null) throw new InvalidOperationException("Savings transaction not found.");

                savingsTranData.Status = 1;

                await _context.SaveChangesAsync();

                investment.ReturnTransactionId = returnTransaction.TransactionId;
                investment.RelializedAmount = request.AmountReturned - savingsTranData.Amount;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ReturnTransactionResponseDTO> GetReturnSavingsTransaction(int returnTransactionId)
        {
            var result = await _context.SavingsTransactions
            .Where(st => st.TransactionId == returnTransactionId)
            .Select(st => new ReturnTransactionResponseDTO
            {
                TransactionId = st.TransactionId,
                Description = st.Description,
                Amount = st.Amount,
                DateUsed = st.DateUsed
            }).FirstOrDefaultAsync();

            if (result == null)
                throw new InvalidOperationException($"Transaction with ID {returnTransactionId} not found.");

            return result;

        }
    }
}
