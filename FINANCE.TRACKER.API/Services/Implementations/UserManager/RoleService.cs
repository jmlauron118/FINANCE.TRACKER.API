using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public RoleService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetAllRoles(int status)
        {
            return await _context.Roles
                .Where(r => status == 2 || r.IsActive == status)
                .Select(r => new RoleResponseDTO
                {
                    RoleId = r.RoleId,
                    Role = r.Role,
                    Description = r.Description,
                    IsActive = r.IsActive
                }).ToListAsync();
        }

        public async Task<RoleResponseDTO> GetRoleById(int id)
        {
            var role = await _context.Roles
                .Where(r => r.RoleId == id)
                .Select(r => new RoleResponseDTO
                {
                    RoleId = r.RoleId,
                    Role = r.Role,
                    Description = r.Description,
                    IsActive = r.IsActive
                }).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new InvalidOperationException("Role not found");
            }

            return role;
        }

        public async Task<RoleResponseDTO> AddRole(RoleRequestDTO role)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Role == role.Role);

            if (existingRole != null)
            {
                throw new InvalidOperationException("Role already exists");
            }

            var newRole = new RoleModel
            {
                Role = role.Role,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.Roles.AddAsync(newRole);
            await _context.SaveChangesAsync();

            return await GetRoleById(newRole.RoleId);
        }

        public async Task<RoleResponseDTO> ModifyRole(RoleModifyDTO role)
        {
            var existingRole = _context.Roles.FirstOrDefault(r => r.Role == role.Role && r.RoleId != role.RoleId);

            if (existingRole != null)
            {
                throw new InvalidOperationException("Role already exists");
            }

            var roleToUpdate = await _context.Roles.FindAsync(role.RoleId);

            if (roleToUpdate == null)
            {
                throw new InvalidOperationException("Role not found");
            }

            roleToUpdate.Role = role.Role;
            roleToUpdate.Description = role.Description;
            roleToUpdate.IsActive = role.IsActive;
            roleToUpdate.UpdatedBy = _userId;
            roleToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetRoleById(roleToUpdate.RoleId);
        }
    }
}
