using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class ModuleService : IModuleService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public ModuleService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        public async Task<IEnumerable<ModuleResponseDTO>> GetAllModules(int status)
        {
            var allModules = await (from modules in _context.Modules
                                    join parent in _context.Modules on modules.ParentId equals parent.ModuleId into parentModuleGroup
                                    from parentModule in parentModuleGroup.DefaultIfEmpty()
                                    where status == 2 || modules.IsActive == status
                                    select new ModuleResponseDTO
                                    {
                                        ModuleId = modules.ModuleId,
                                        ModuleName = modules.ModuleName,
                                        Description = modules.Description,
                                        ModulePage = modules.ModulePage,
                                        Icon = modules.Icon,
                                        SortOrder = modules.SortOrder,
                                        IsActive = modules.IsActive,
                                        ParentId = modules.ParentId,
                                        ParentModule = parentModule != null ? parentModule.ModuleName : null
                                    })
                                    .OrderBy(m => m.ParentId)
                                    .ThenBy(m => m.SortOrder)
                                    .ToListAsync();

            return allModules;
        }

        public async Task<ModuleResponseDTO> GetModuleById(int id)
        {
            var module = await _context.Modules
                .Where(m => m.ModuleId == id)
                .GroupJoin(
                    _context.Modules,
                    m => m.ParentId,
                    p => p.ModuleId,
                    (m, parents) => new { m, parents }
                )
                .SelectMany(
                    x => x.parents.DefaultIfEmpty(),
                    (x, p) => new ModuleResponseDTO
                    {
                        ModuleId = x.m.ModuleId,
                        ModuleName = x.m.ModuleName,
                        Description = x.m.Description,
                        ModulePage = x.m.ModulePage,
                        Icon = x.m.Icon,
                        SortOrder = x.m.SortOrder,
                        IsActive = x.m.IsActive,
                        ParentId = x.m.ParentId,
                        ParentModule = x.m.ParentId == 0 ? null : p.ModuleName
                    }
                )
                .FirstOrDefaultAsync();

            if (module == null)
            {
                throw new InvalidOperationException("Module not found");
            }

            return module;
        }

        public async Task<IEnumerable<ModuleResponseDTO>> GetAllParentModules()
        {
            return await _context.Modules
                    .OrderBy(m => m.SortOrder)
                    .Where(m => m.ParentId == 0 && m.IsActive == 1)
                    .Select(m => new ModuleResponseDTO
                    {
                        ModuleId = m.ModuleId,
                        ModuleName = m.ModuleName,
                        Description = m.Description,
                        ModulePage = m.ModulePage,
                        Icon = m.Icon
                    }).ToListAsync();
        }

        public async Task<ModuleResponseDTO> AddModule(ModuleRequestDTO module)
        {
            var existingModule = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleName == module.ModuleName || m.ModulePage == module.ModulePage);

            if (existingModule != null)
            {
                throw new InvalidOperationException("Module already exists");
            }

            var newModule = new ModuleModel
            {
                ModuleName = module.ModuleName,
                ModulePage = module.ModulePage,
                Description = module.Description,
                Icon = module.Icon,
                SortOrder = module.SortOrder,
                IsActive = module.IsActive,
                ParentId = module.ParentId,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.Modules.AddAsync(newModule);
            await _context.SaveChangesAsync();

            return await GetModuleById(newModule.ModuleId);
        }

        public async Task<ModuleResponseDTO> ModifyModule(ModuleModifyDTO module)
        {
            var existingModule = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleName == module.ModuleName && m.ModuleId != module.ModuleId);

            if (existingModule != null)
            {
                throw new InvalidOperationException("Module name already exists");
            }

            var moduleToUpdate = await _context.Modules.FindAsync(module.ModuleId);

            if (moduleToUpdate == null)
            {
                throw new InvalidOperationException("Module not found");
            }

            moduleToUpdate.ModuleName = module.ModuleName;
            moduleToUpdate.ModulePage = module.ModulePage;
            moduleToUpdate.Description = module.Description;
            moduleToUpdate.Icon = module.Icon;
            moduleToUpdate.SortOrder = module.SortOrder;
            moduleToUpdate.IsActive = module.IsActive;
            moduleToUpdate.ParentId = module.ParentId;
            moduleToUpdate.UpdatedBy = _userId;
            moduleToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetModuleById(moduleToUpdate.ModuleId);    
        }
    }
}
