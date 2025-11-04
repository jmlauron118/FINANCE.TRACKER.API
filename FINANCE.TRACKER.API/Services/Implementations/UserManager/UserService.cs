using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponseDTO>> GellAllUsers(int status)
        {
            return await _context.Users
                .Where(u => status == 2 || u.IsActive == status)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Gender = u.Gender,
                    Username = u.Username,
                    IsActive = u.IsActive
                }).ToListAsync();
        }

        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Gender = u.Gender,
                    Username = u.Username,
                    IsActive = u.IsActive
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return user;
        }

        public async Task<UserResponseDTO> AddUser(UserRequestDTO user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            var hasher = new PasswordHasher<UserRequestDTO>();

            user.Password = hasher.HashPassword(user, user.Password);

            var newUser = new UserModel
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Gender = user.Gender,
                Username = user.Username,
                Password = user.Password,
                IsActive = user.IsActive,
                CreatedBy = 1, // TODO: Replace with actual user ID
                DateCreated = DateTime.Now
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return await GetUserById(newUser.UserId);
        }

        public async Task<UserResponseDTO> ModifyUser(UserModifyDTO user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.UserId != user.UserId);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            var userToUpdate = await _context.Users.FindAsync(user.UserId);

            if (userToUpdate == null)
            {
                throw new InvalidOperationException("User not found");
            }

            userToUpdate.Firstname = user.Firstname;
            userToUpdate.Lastname = user.Lastname;
            userToUpdate.Gender = user.Gender;
            userToUpdate.Username = user.Username;
            userToUpdate.IsActive = user.IsActive;
            userToUpdate.UpdatedBy = 1; // TODO: Replace with actual user ID
            userToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetUserById(userToUpdate.UserId);
        }

        public async Task<IEnumerable<UserModuleResponseDTO>> GetUserModules(int userId)
        {
            var userModules = await (from moduleAccess in _context.ModuleAccesses
                                     join moduleAction in _context.ModuleActions on moduleAccess.ModuleActionId equals moduleAction.ModuleActionId
                                     join module in _context.Modules on moduleAction.ModuleId equals module.ModuleId
                                     join action in _context.Actions on moduleAction.ActionId equals action.ActionId
                                     join userRole in _context.UserRoles on moduleAccess.UserRoleId equals userRole.UserRoleId
                                     join user in _context.Users on userRole.UserId equals user.UserId
                                     join role in _context.Roles on userRole.RoleId equals role.RoleId
                                     where module.IsActive == 1 && action.IsActive == 1 && user.IsActive == 1 && role.IsActive == 1 && user.UserId == userId
                                     orderby module.SortOrder
                                     select new UserModuleResponseDTO
                                     {
                                         ModuleId = module.ModuleId,
                                         ModuleName = module.ModuleName,
                                         ModulePage = module.ModulePage,
                                         Icon = module.Icon,
                                         SortNo = module.SortOrder
                                     }).ToListAsync();

            var uniqueModules = userModules
                .GroupBy(m => m.ModuleId)
                .Select(g => g.First())
                .OrderBy(m => m.SortNo)
                .ToList();

            return uniqueModules;
        }
    }
}
