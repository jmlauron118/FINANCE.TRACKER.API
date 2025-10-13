using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _context;

        public UserRoleService(AppDbContext context)
        {
            _context = context;
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
            var existingUserRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);

            if (existingUserRole != null)
            {
                throw new InvalidOperationException("User role already exists");
            }

            var newUserRole = new UserRoleModel
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
                CreatedBy = 1, // TODO: Replace with actual user ID
                DateCreated = DateTime.Now
            };

            await _context.UserRoles.AddAsync(newUserRole);
            await _context.SaveChangesAsync();

            return await GetUserRoleById(newUserRole.UserRoleId);
        }

        public async Task<UserRoleResponseDTO> ModifyUserRole(UserRoleModifyDTO userRole)
        {
            var existingUserRole = await _context.UserRoles.FirstOrDefaultAsync(ur => (ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId) && ur.UserRoleId == userRole.UserRoleId);

            if (existingUserRole != null)
            {
                throw new InvalidOperationException("User role already exists");
            }

            var userRoleToUpdate = await _context.UserRoles.FindAsync(userRole.UserRoleId);

            if (userRoleToUpdate == null)
            {
                throw new InvalidOperationException("User role not found");
            }

            userRoleToUpdate.UserId = userRole.UserId;
            userRoleToUpdate.RoleId = userRole.RoleId;
            userRoleToUpdate.UpdatedBy = 1; // TODO: Replace with actual user ID
            userRoleToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetUserRoleById(userRoleToUpdate.UserRoleId);
        }
    }
}
