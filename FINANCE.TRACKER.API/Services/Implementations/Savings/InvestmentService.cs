using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;
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

        public Task<IEnumerable<InvestmentResponseDTO>> GetAllInvestments()
        {
            throw new NotImplementedException();
        }
    }
}
