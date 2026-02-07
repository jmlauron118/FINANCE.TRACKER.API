using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;
using FINANCE.TRACKER.API.Models.Savings;
using FINANCE.TRACKER.API.Services.Interfaces.Savings;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Savings
{
    public class SavingsService : ISavingsService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userid;

        public SavingsService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userid = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        }

        public enum TransactionType
        {
            Expenses = 1,
            Investment = 2
        }

        public async Task<IEnumerable<SavingsTransactionResponseDTO>> GetAllSavingsTransactions()
        {
            return await (from st in _context.SavingsTransactions
                          join stt in _context.SavingsTransactionTypes on st.TransactionTypeId equals stt.TransactionTypeId
                          join i in _context.Investments on st.TransactionId equals i.TransactionId into iGroup
                          from ig in iGroup.DefaultIfEmpty() 
                          where st.CreatedBy == _userid
                          select new SavingsTransactionResponseDTO
                          {
                              TransactionId = st.TransactionId,
                              TransactionTypeId = stt.TransactionTypeId,
                              TransactionTypeName = stt.TransactionTypeName,
                              InvestmentTypeId = ig != null ? ig.InvestmentTypeId : (int?)null,
                              Description = st.Description,
                              Amount = st.Amount,
                              Status = st.Status,
                              DateUsed = st.DateUsed
                          }).ToListAsync();
        }

        public async Task AddSavingsTransaction(SavingsTransactionRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newTransaction = new SavingsTransactionModel
                {
                    TransactionTypeId = request.TransactionTypeId,
                    Description = request.Description,
                    Amount = request.Amount,
                    DateUsed = request.DateUsed,
                    Status = request.Status,
                    CreatedBy = _userid,
                    DateCreated = DateTime.Now
                };

                await _context.SavingsTransactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync();

                if (request.TransactionTypeId == (int)TransactionType.Investment) 
                {
                    var newInvestment = new InvestmentModel
                    {
                        TransactionId = newTransaction.TransactionId,
                        InvestmentTypeId = request.InvestmentTypeId ?? 0,
                        CreatedBy = _userid,
                        DateCreated = DateTime.Now
                    };

                    await _context.Investments.AddAsync(newInvestment);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch 
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ModifySavingsTransaction(SavingsTransactionModifyDTO request)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var transactionToUpdate = await _context.SavingsTransactions.FindAsync(request.TransactionId);

                if (transactionToUpdate == null) throw new InvalidOperationException("Savings transaction not found!");

                transactionToUpdate.TransactionTypeId = request.TransactionTypeId;
                transactionToUpdate.Description = request.Description;
                transactionToUpdate.Amount = request.Amount;
                transactionToUpdate.DateUsed = request.DateUsed;
                transactionToUpdate.Status = request.Status;

                await _context.SaveChangesAsync();

                if (request.TransactionTypeId == (int)TransactionType.Investment)
                {
                    var investmentToUpdate = await _context.Investments.SingleOrDefaultAsync(i => i.TransactionId == request.TransactionId);

                    if (investmentToUpdate == null) throw new InvalidOperationException("Investment details not found!");

                    investmentToUpdate.InvestmentTypeId = request.InvestmentTypeId ?? 0;

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RemoveSavingsTransaction(int transactionId)
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var transactionToDelete = await _context.SavingsTransactions
                    .FindAsync(transactionId);

                if (transactionToDelete == null) throw new InvalidOperationException("Savings transaction not found.");

                if (transactionToDelete.TransactionTypeId == (int)TransactionType.Investment) 
                {
                    var investmentToDelete = await _context.Investments.SingleOrDefaultAsync(i => i.TransactionId == transactionId);

                    if (investmentToDelete == null) throw new InvalidOperationException("Investment details not found.");

                    _context.Investments.Remove(investmentToDelete);
                }

                _context.SavingsTransactions.Remove(transactionToDelete);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

    }
}
