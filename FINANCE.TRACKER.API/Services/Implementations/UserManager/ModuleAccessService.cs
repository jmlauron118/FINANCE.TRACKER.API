using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleAccessDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class ModuleAccessService : IModuleAccessService
    {
        private readonly AppDbContext _context;

        public ModuleAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ModuleAccessResponseDTO>> GetAllModuleAccess()
        {
            var moduleAcessList = from mac in _context.ModuleAccesses
                                  join ma in _context.ModuleActions on mac.ModuleActionId equals ma.ModuleActionId
                                  join ur in _context.UserRoles on mac.UserRoleId equals ur.UserRoleId
                                  join m in _context.Modules on ma.ModuleId equals m.ModuleId
                                  join a in _context.Actions on ma.ActionId equals a.ActionId
                                  join u in _context.Users on ur.UserId equals u.UserId
                                  join r in _context.Roles on ur.RoleId equals r.RoleId
                                  where m.IsActive == 1 && a.IsActive == 1 && u.IsActive == 1 && r.IsActive == 1
                                  select new ModuleAccessResponseDTO
                                  {
                                      ModuleAccessId = mac.ModuleAccessId,
                                      ModuleActionId = ma.ModuleActionId,
                                      ModuleName = m.ModuleName,
                                      ActionName = a.ActionName,
                                      UserRoleId = ur.UserRoleId,
                                      Username = u.Username,
                                      Role = r.Role
                                  };

            return await moduleAcessList.ToListAsync();
        }

        public async Task<ModuleAccessResponseDTO> GetModuleAccessById(int id)
        {
            var moduleAccess = from mac in _context.ModuleAccesses
                               join ma in _context.ModuleActions on mac.ModuleActionId equals ma.ModuleActionId
                               join ur in _context.UserRoles on mac.UserRoleId equals ur.UserRoleId
                               join m in _context.Modules on ma.ModuleId equals m.ModuleId
                               join a in _context.Actions on ma.ActionId equals a.ActionId
                               join u in _context.Users on ur.UserId equals u.UserId
                               join r in _context.Roles on ur.RoleId equals r.RoleId
                               where mac.ModuleAccessId == id
                               select new ModuleAccessResponseDTO
                               {
                                   ModuleAccessId = mac.ModuleAccessId,
                                   ModuleActionId = ma.ModuleActionId,
                                   ModuleName = m.ModuleName,
                                   ActionName = a.ActionName,
                                   UserRoleId = ur.UserRoleId,
                                   Username = u.Username,
                                   Role = r.Role
                               };

            var result = await moduleAccess.FirstOrDefaultAsync();

            if (result == null)
            {
                throw new InvalidOperationException("Module access not found");
            }

            return result;
        }
        public async Task<ModuleAccessResponseDTO> AddModuleAccess(ModuleAccessRequestDTO moduleAcess)
        {
            var existingModuleAccess = await _context.ModuleAccesses
                .FirstOrDefaultAsync(mac => mac.ModuleActionId == moduleAcess.ModuleActionId && mac.UserRoleId == moduleAcess.UserRoleId);

            if (existingModuleAccess != null)
            {
                throw new InvalidOperationException("Module access already exists");
            }

            var newModuleAccess = new ModuleAccessModel
            {
                ModuleActionId = moduleAcess.ModuleActionId,
                UserRoleId = moduleAcess.UserRoleId,
                CreatedBy = 1, // TODO: Replace with actual user ID
                DateCreated = DateTime.Now
            };

            await _context.ModuleAccesses.AddAsync(newModuleAccess);
            await _context.SaveChangesAsync();

            return await GetModuleAccessById(newModuleAccess.ModuleAccessId);
        }

        public async Task<ModuleAccessResponseDTO> ModifyModuleAccess(ModuleAccessModifyDTO moduleAccess)
        {
            var existingModuleAccess = await _context.ModuleAccesses
                .FirstOrDefaultAsync(mac => (mac.ModuleActionId == moduleAccess.ModuleActionId && mac.UserRoleId == moduleAccess.UserRoleId) && mac.ModuleAccessId != moduleAccess.ModuleAccessId);

            if (existingModuleAccess != null)
            {
                throw new InvalidOperationException("Module access already exists");
            }

            var moduleAccessToUpdate = await _context.ModuleAccesses.FindAsync(moduleAccess.ModuleAccessId);

            if (moduleAccessToUpdate == null)
            {
                throw new InvalidOperationException("Module access not found");
            }

            moduleAccessToUpdate.ModuleActionId = moduleAccess.ModuleActionId;
            moduleAccessToUpdate.UserRoleId = moduleAccess.UserRoleId;
            moduleAccessToUpdate.UpdatedBy = 1; // TODO: Replace with actual user ID
            moduleAccessToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetModuleAccessById(moduleAccessToUpdate.ModuleAccessId);
        }

        public async Task RemoveModuleAccess(int id)
        {
            var moduleAccessToDelete = await _context.ModuleAccesses.FindAsync(id);

            if (moduleAccessToDelete == null)
            {
                throw new InvalidOperationException("Module access not found");
            }

            _context.ModuleAccesses.Remove(moduleAccessToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
