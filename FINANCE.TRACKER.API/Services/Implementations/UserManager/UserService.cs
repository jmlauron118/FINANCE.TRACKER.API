using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.Auth;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public UserService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers(int status)
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
                CreatedBy = _userId,
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
            userToUpdate.UpdatedBy = _userId;
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
                                     join c in _context.Modules on module.ModuleId equals c.ParentId into childModules
                                     where 
                                        module.IsActive == 1 && 
                                        action.IsActive == 1 && 
                                        user.IsActive == 1 && 
                                        role.IsActive == 1 && 
                                        user.UserId == userId &&
                                        action.ActionName == "CAN_VIEW"
                                     orderby module.ParentId, module.SortOrder
                                     select new UserModuleResponseDTO
                                     {
                                         ModuleId = module.ModuleId,
                                         ModuleName = module.ModuleName,
                                         ModulePage = module.ModulePage,
                                         Icon = module.Icon,
                                         SortOrder = module.SortOrder,
                                         ParentId = module.ParentId,
                                         ChildCount = childModules.Count()
                                     }).ToListAsync();

            return SortModuleHierarchy(userModules);
        }

        public async Task ChangePassword(ChangePasswordModel changePassword)
        {
            var userId = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)? id : 0;
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new InvalidOperationException("User not found!");

            var hasher = new PasswordHasher<UserModel>();
            var result = hasher.VerifyHashedPassword(user, user.Password, changePassword.CurrentPassword);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Current password is incorrect!");
            }

            var isSame = hasher.VerifyHashedPassword(user, user.Password, changePassword.NewPassword);

            if (isSame == PasswordVerificationResult.Success)
            {
                throw new InvalidOperationException("New password must be different from the current password.");
            }

            user.Password = hasher.HashPassword(user, changePassword.NewPassword);
            user.UpdatedBy = userId;
            user.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        private List<UserModuleResponseDTO> SortModuleHierarchy(List<UserModuleResponseDTO> modules)
        {
            var lookup = modules.GroupBy(m => m.ParentId)
                                .ToDictionary(g => g.Key, g => g.OrderBy(m => m.SortOrder).ToList());

            var result = new List<UserModuleResponseDTO>();

            void AddModules(int parentId)
            {
                if(!lookup.ContainsKey(parentId))
                {
                    return;
                }

                foreach(var module in lookup[parentId])
                {
                    result.Add(module);
                    AddModules(module.ModuleId);
                }
            }

            AddModules(0);

            return result;
        }
    }
}
