using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.Auth;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.UserManager
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
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
                CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
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
            userToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
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
    }
}
