using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class ActionService : IActionService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ActionService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<IEnumerable<ActionResponseDTO>> GetAllActions(int status)
        {
            return await _context.Actions
                .Where(a => status == 2 || a.IsActive == status)
                .Select(a => new ActionResponseDTO
                {
                    ActionId = a.ActionId,
                    ActionName = a.ActionName,
                    Description = a.Description,
                    IsActive = a. IsActive
                }).ToListAsync();
        }

        public async Task<ActionResponseDTO> GetActionById(int id)
        {
            var action = await _context.Actions
                .Where(a => a.ActionId == id)
                .Select(a => new ActionResponseDTO
                {
                    ActionId = a.ActionId,
                    ActionName = a.ActionName,
                    Description = a.Description,
                    IsActive = a.IsActive
                }).FirstOrDefaultAsync();

            if (action == null)
            {
                throw new InvalidOperationException("Action not found");
            }

            return action;
        }
        public async Task<ActionResponseDTO> AddAction(ActionRequestDTO action)
        {
            var existingAction = await _context.Actions.FirstOrDefaultAsync(a => a.ActionName == action.ActionName);

            if (existingAction != null)
            {
                throw new InvalidOperationException("Action already exists");
            }

            var newAction = new ActionModel
            {
                ActionName = action.ActionName,
                Description = action.Description,
                IsActive = action.IsActive,
                CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
                DateCreated = DateTime.Now
            };

            await _context.Actions.AddAsync(newAction);
            await _context.SaveChangesAsync();

            return await GetActionById(newAction.ActionId);
        }

        public async Task<ActionResponseDTO> ModifyAction(ActionModityDTO action)
        {
            var existingAction = await _context.Actions.FirstOrDefaultAsync(a => a.ActionName == action.ActionName && a.ActionId != action.ActionId);

            if (existingAction != null)
            {
                throw new InvalidOperationException("Action already exists");
            }

            var actionToUpdate = await _context.Actions.FindAsync(action.ActionId);

            if (actionToUpdate == null)
            {
                throw new InvalidOperationException("Action not found");
            }

            actionToUpdate.ActionName = action.ActionName;
            actionToUpdate.Description = action.Description;
            actionToUpdate.IsActive = action.IsActive;
            actionToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            actionToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetActionById(actionToUpdate.ActionId);
        }
    }
}
