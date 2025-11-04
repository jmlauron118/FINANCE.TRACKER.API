using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleActionDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class ModuleActionService : IModuleActionService
    {
        private readonly AppDbContext _context;

        public ModuleActionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ModuleActionResponseDTO>> GetAllModuleActions()
        {
            var moduleActions = from ma in _context.ModuleActions
                                join m in _context.Modules on ma.ModuleId equals m.ModuleId
                                join a in _context.Actions on ma.ActionId equals a.ActionId
                                select new ModuleActionResponseDTO
                                {
                                    ModuleActionId = ma.ModuleActionId,
                                    ModuleId = m.ModuleId,
                                    ModuleName = m.ModuleName,
                                    ModuleDescription = m.Description,
                                    Icon = m.Icon,
                                    ActionId = a.ActionId,
                                    ActionName = a.ActionName,
                                    ActionDescription = a.Description
                                };

            return await moduleActions.ToListAsync();
        }

        public async Task<ModuleActionResponseDTO> GetModuleActionById(int id)
        {
            var moduleAction = from ma in _context.ModuleActions
                               join m in _context.Modules on ma.ModuleId equals m.ModuleId
                               join a in _context.Actions on ma.ActionId equals a.ActionId
                               where ma.ModuleActionId == id
                               select new ModuleActionResponseDTO
                               {
                                   ModuleActionId = ma.ModuleActionId,
                                   ModuleId = m.ModuleId,
                                   ModuleName = m.ModuleName,
                                   ModuleDescription = m.Description,
                                   Icon = m.Icon,
                                   ActionId = a.ActionId,
                                   ActionName = a.ActionName,
                                   ActionDescription = a.Description
                               };

            var result = await moduleAction.FirstOrDefaultAsync();

            if (result == null)
            {
                throw new InvalidOperationException("Module action not found");
            }

            return result;
        }
        public async Task<ModuleActionResponseDTO> AddModuleAcction(ModuleActionRequestDTO moduleAction)
        {
            var existingModuleAction = _context.ModuleActions.FirstOrDefault(ma => ma.ModuleId == moduleAction.ModuleId && ma.ActionId == moduleAction.ActionId);

            if (existingModuleAction != null)
            {
                throw new InvalidOperationException("Module action already exists");
            }

            var newModuleAction = new ModuleActionModel
            {
                ModuleId = moduleAction.ModuleId,
                ActionId = moduleAction.ActionId,
                CreatedBy = 1, // TODO: Replace with actual user ID
                DateCreated = DateTime.Now
            };

            await _context.ModuleActions.AddAsync(newModuleAction);
            await _context.SaveChangesAsync();

            return await GetModuleActionById(newModuleAction.ModuleActionId);
        }

        public async Task<ModuleActionResponseDTO> ModifyModuleAction(ModuleActionModifyDTO moduleAction)
        {
            var existingModuleAction = await _context.ModuleActions.FirstOrDefaultAsync(ma => (ma.ModuleId == moduleAction.ModuleId && ma.ActionId == moduleAction.ActionId) && ma.ModuleActionId != moduleAction.ModuleActionId);

            if (existingModuleAction != null)
            {
                throw new InvalidOperationException("Module action already exists");
            }

            var moduleActionToUpdate = await _context.ModuleActions.FindAsync(moduleAction.ModuleActionId);

            if (moduleActionToUpdate == null)
            {
                throw new InvalidOperationException("Module action not found");
            }

            moduleActionToUpdate.ModuleId = moduleAction.ModuleId;
            moduleActionToUpdate.ActionId = moduleAction.ActionId;
            moduleActionToUpdate.UpdatedBy = 1; // TODO: Replace with actual user ID
            moduleActionToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetModuleActionById(moduleActionToUpdate.ModuleActionId);
        }

        public async Task RemoveModuleAction(int id)
        {
            var moduleActionToDelete = await _context.ModuleActions.FindAsync(id);

            if (moduleActionToDelete != null)
            {
                var moduleAccess = await _context.ModuleAccesses.FirstOrDefaultAsync(mac => mac.ModuleActionId == id);

                if (moduleAccess != null)
                {
                    _context.ModuleAccesses.Remove(moduleAccess);
                }

                _context.ModuleActions.Remove(moduleActionToDelete);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Module action not found");
            }
        }
    }
}
