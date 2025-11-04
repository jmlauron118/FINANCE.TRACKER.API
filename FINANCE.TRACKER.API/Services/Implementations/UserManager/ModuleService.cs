using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class ModuleService : IModuleService
    {
        private readonly AppDbContext _context;

        public ModuleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ModuleResponseDTO>> GetAllModules(int status)
        {
            return await _context.Modules
                .Where(m => status == 2 || m.IsActive == status)
                .Select(m => new ModuleResponseDTO
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    ModulePage = m.ModulePage,
                    Icon = m.Icon,
                    SortOrder = m.SortOrder,
                    IsActive = m.IsActive
                }).OrderBy(m => m.SortOrder).ToListAsync();
        }

        public async Task<ModuleResponseDTO> GetModuleById(int id)
        {
            var module = await _context.Modules
                .Where(m => m.ModuleId == id)
                .Select(m => new ModuleResponseDTO
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    ModulePage = m.ModulePage,
                    Icon = m.Icon,
                    SortOrder = m.SortOrder,
                    IsActive = m.IsActive
                }).FirstOrDefaultAsync();

            if (module == null)
            {
                throw new InvalidOperationException("Module not found");
            }

            return module;
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
                CreatedBy = 1, // TODO: Replace with actual user ID
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
            moduleToUpdate.UpdatedBy = 1; // TODO: Replace with actual user ID
            moduleToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetModuleById(moduleToUpdate.ModuleId);    
        }
    }
}
