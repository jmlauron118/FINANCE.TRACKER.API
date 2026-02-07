using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionTypeDTO;
using FINANCE.TRACKER.API.Models.Savings;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Category
{
    public class SavingsTransactionTypeService : ISavingsTransactionTypeService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public SavingsTransactionTypeService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        }

        public async Task<IEnumerable<SavingsTransactionTypeResponseDTO>> GetAllSavingsTransactionTypes(int status, int type)
        {
            return await _context.SavingsTransactionTypes
                .Where(st => (status == 2 || st.IsActive == status) && (type == 2 || st.Direction == type))
                .Select(st => new SavingsTransactionTypeResponseDTO
                {
                    TransactionTypeId = st.TransactionTypeId,
                    TransactionTypeName = st.TransactionTypeName,
                    Description = st.Description,
                    Direction = st.Direction,
                    IsActive = st.IsActive
                }).ToListAsync();
        }

        public async Task<SavingsTransactionTypeResponseDTO> GetSavingsTransactionTypeById(int id)
        {
            var transactionType = await _context.SavingsTransactionTypes
                .Where(st => st.TransactionTypeId == id)
                .Select(st => new SavingsTransactionTypeResponseDTO
                {
                    TransactionTypeId = st.TransactionTypeId,
                    TransactionTypeName = st.TransactionTypeName,
                    Description = st.Description,
                    Direction = st.Direction,
                    IsActive = st.IsActive
                }).FirstOrDefaultAsync();

            if (transactionType == null) throw new InvalidOperationException("Savings transaction type not found!");

            return transactionType;
        }

        public async Task<SavingsTransactionTypeResponseDTO> AddSavingsTransactionType(SavingsTransactionTypeRequestDTO request)
        {
            var existingTransactionType = await _context.SavingsTransactionTypes.FirstOrDefaultAsync(st => st.TransactionTypeName == request.TransactionTypeName);

            if (existingTransactionType != null) throw new InvalidOperationException("Savings transaction type already exist!");

            var newTransactionType = new SavingsTransactionTypeModel
            {
                TransactionTypeName = request.TransactionTypeName,
                Description = request.Description,
                Direction = request.Direction,
                IsActive = request.IsActive,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.SavingsTransactionTypes.AddAsync(newTransactionType);
            await _context.SaveChangesAsync();

            return await this.GetSavingsTransactionTypeById(newTransactionType.TransactionTypeId);
        }

        public async Task<SavingsTransactionTypeResponseDTO> ModifySavingsTransactionType(SavingsTransactionTypeModifyDTO request)
        {
            var existingTransactionType = await _context.SavingsTransactionTypes
                .FirstOrDefaultAsync(st => st.TransactionTypeName == request.TransactionTypeName && st.TransactionTypeId != request.TransactionTypeId);

            if (existingTransactionType != null) throw new InvalidOperationException("Savings transaction type already exist!");

            var transactionTypeToUpdate = await _context.SavingsTransactionTypes.FindAsync(request.TransactionTypeId);

            if (transactionTypeToUpdate == null) throw new InvalidOperationException("Savings transaction type not found!");

            transactionTypeToUpdate.TransactionTypeName = request.TransactionTypeName;
            transactionTypeToUpdate.Description = request.Description;
            transactionTypeToUpdate.Direction = request.Direction;
            transactionTypeToUpdate.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return await this.GetSavingsTransactionTypeById(transactionTypeToUpdate.TransactionTypeId);
        }
    }
}
