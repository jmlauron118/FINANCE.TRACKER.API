using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserRoleService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<UserRoleResponseDTO>> GetAllUserRoles()
        {
            var userRoles = from ur in _context.UserRoles
                   join u in _context.Users on ur.UserId equals u.UserId
                   join r in _context.Roles on ur.RoleId equals r.RoleId
                   select new UserRoleResponseDTO
                   {
                       UserRoleId = ur.UserRoleId,
                       UserId = u.UserId,
                       Fullname = u.Lastname + ", " + u.Firstname,
                       Username = u.Username,
                       RoleId = r.RoleId,
                       Role = r.Role
                   };

            return await userRoles.ToListAsync();
        }

        public async Task<UserRoleResponseDTO> GetUserRoleById(int id)
        {
            var userRole = from ur in _context.UserRoles
                           join u in _context.Users on ur.UserId equals u.UserId
                           join r in _context.Roles on ur.RoleId equals r.RoleId
                           where ur.UserRoleId == id
                           select new UserRoleResponseDTO
                           {
                               UserRoleId = ur.UserRoleId,
                               UserId = u.UserId,
                               Fullname = u.Lastname + ", " + u.Firstname,
                               Username = u.Username,
                               RoleId = r.RoleId,
                               Role = r.Role
                           };

            var result = await userRole.FirstOrDefaultAsync();

            if (result == null)
            {
                throw new InvalidOperationException("User role not found");
            }

            return result;
        }

        public async Task<UserRoleResponseDTO> AddUserRole(UserRoleRequestDTO userRole)
        {
            var existingUserRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId);

            if (existingUserRole != null)
            {
                throw new InvalidOperationException("The selected user already has a registered role.");
            }

            var newUserRole = new UserRoleModel
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
                CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
                DateCreated = DateTime.Now
            };

            await _context.UserRoles.AddAsync(newUserRole);
            await _context.SaveChangesAsync();

            return await GetUserRoleById(newUserRole.UserRoleId);
        }

        public async Task<UserRoleResponseDTO> ModifyUserRole(UserRoleModifyDTO userRole)
        {
            var existingUserRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.UserRoleId != userRole.UserRoleId);

            if (existingUserRole != null)
            {
                throw new InvalidOperationException("The selected user already has a registered role.");
            }

            var userRoleToUpdate = await _context.UserRoles.FindAsync(userRole.UserRoleId);

            if (userRoleToUpdate == null)
            {
                throw new InvalidOperationException("User role not found");
            }

            userRoleToUpdate.UserId = userRole.UserId;
            userRoleToUpdate.RoleId = userRole.RoleId;
            userRoleToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            userRoleToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetUserRoleById(userRoleToUpdate.UserRoleId);
        }

        public async Task RemoveUserRole(int id)
        {
            var userRoleToDelete = await _context.UserRoles.FindAsync(id);

            if(userRoleToDelete != null)
            {
                var moduleAccess = await _context.ModuleAccesses.FirstOrDefaultAsync(mac => mac.UserRoleId == id);

                if(moduleAccess != null)
                {
                    _context.ModuleAccesses.Remove(moduleAccess);
                }

                _context.UserRoles.Remove(userRoleToDelete);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("User role not found");
            }
        }
    }
}
