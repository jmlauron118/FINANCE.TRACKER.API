using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentTypeDTO;
using FINANCE.TRACKER.API.Models.Savings;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Category
{
    public class InvestmentTypeService : IInvestmentTypeService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public InvestmentTypeService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? 1: 0;
        }

        public async Task<IEnumerable<InvestmentTypeResponseDTO>> GetAllInvestmentTypes(int status)
        {
            return await _context.InvestmentTypes
                .Where(it => status == 2 || it.IsActive == status)
                .Select(it => new InvestmentTypeResponseDTO
                {
                    InvestmentTypeId = it.InvestmentTypeId,
                    InvestmentTypeName = it.InvestmentTypeName,
                    Description = it.Description,
                    IsActive = it.IsActive
                }).ToListAsync();
        }

        public async Task<InvestmentTypeResponseDTO> GetInvestmentTypeById(int id)
        {
            var investmentType = await _context.InvestmentTypes
                .Where(it => it.InvestmentTypeId == id)
                .Select(it => new InvestmentTypeResponseDTO
                {
                    InvestmentTypeId = it.InvestmentTypeId,
                    InvestmentTypeName = it.InvestmentTypeName,
                    Description = it.Description,
                    IsActive = it.IsActive
                }).FirstOrDefaultAsync();

            if (investmentType == null) throw new InvalidOperationException("Investment type not found!");

            return investmentType;
        }

        public async Task<InvestmentTypeResponseDTO> AddInvestmentType(InvestmentTypeRequestDTO request)
        {
            var existingInvestmentType = await _context.InvestmentTypes.FirstOrDefaultAsync(it => it.InvestmentTypeName == request.InvestmentTypeName);

            if (existingInvestmentType != null) throw new InvalidOperationException("Investment type already exist!");

            var newInvestmentType = new InvestmentTypeModel
            {
                InvestmentTypeName = request.InvestmentTypeName,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.InvestmentTypes.AddAsync(newInvestmentType);
            await _context.SaveChangesAsync();

            return await this.GetInvestmentTypeById(newInvestmentType.InvestmentTypeId);
        }
        public async Task<InvestmentTypeResponseDTO> ModifyInvestmentType(InvestmentTypeModifyDTO request)
        {
            var existingInvestmentType = await _context.InvestmentTypes
                .FirstOrDefaultAsync(it => it.InvestmentTypeName == request.InvestmentTypeName && it.InvestmentTypeId != request.InvestmentTypeId);

            if (existingInvestmentType != null) throw new InvalidOperationException("Investment type already exist!");

            var investmentTypeToUpdate = await _context.InvestmentTypes.FindAsync(request.InvestmentTypeId);

            if (investmentTypeToUpdate == null) throw new InvalidOperationException("Investment type not found!");

            investmentTypeToUpdate.InvestmentTypeName = request.InvestmentTypeName;
            investmentTypeToUpdate.Description = request.Description;
            investmentTypeToUpdate.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return await this.GetInvestmentTypeById(investmentTypeToUpdate.InvestmentTypeId);
        }
    }
}
