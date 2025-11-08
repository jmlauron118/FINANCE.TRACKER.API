using FINANCE.TRACKER.API.Models;
using FINANCE.TRACKER.API.Models.Auth;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleAccessDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleActionDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserManagerController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IActionService _actionService;
        private readonly IModuleService _moduleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IModuleActionService _moduleActionService;
        private readonly IModuleAccessService _moduleAccessService;

        public UserManagerController(IUserService userService,
                                        IRoleService roleService,
                                        IActionService actionService,
                                        IModuleService moduleService,
                                        IUserRoleService userRoleService,
                                        IModuleActionService moduleActionService,
                                        IModuleAccessService moduleAccessService)
        {
            _userService = userService;
            _roleService = roleService;
            _actionService = actionService;
            _moduleService = moduleService;
            _userRoleService = userRoleService;
            _moduleActionService = moduleActionService;
            _moduleAccessService = moduleAccessService;
        }

        #region User
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<UserResponseDTO>>
            {
                Data = await _userService.GetAllUsers(status),
                Message = "Users fetched successfully!"
            });
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDTO user)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<UserResponseDTO>
                {
                    Data = await _userService.AddUser(user),
                    Message = "User added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-user")]
        public async Task<IActionResult> ModifyUser([FromBody] UserModifyDTO user)
        {
            try
            {
                return Ok(new ResponseModel<UserResponseDTO>
                {
                    Data = await _userService.ModifyUser(user),
                    Message = "User modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("get-user-modules")]
        public async Task<IActionResult> GetUserModules()
        {
            try
            {
                int userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
                
                if (userId == 0)
                {
                    return NotFound(new { Message = "User not found!"});
                }

                var userModules = await _userService.GetUserModules(userId);

                if(userModules.Count() == 0)
                {
                    return NotFound(new { Message = "No module(s) assigned. Please contact the administrator." });
                }

                return Ok(new ResponseModel<IEnumerable<UserModuleResponseDTO>>
                {
                    Data = userModules,
                    Message = "User modules fetched successfully!"
                });
            }
            catch(InvalidCastException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePassword)
        {
            try
            {
                await _userService.ChangePassword(changePassword);

                return Ok(new ResponseModel<object>
                {
                    Message = "Password changed successfully!"
                });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion User

        #region Role
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<RoleResponseDTO>>
            {
                Data = await _roleService.GetAllRoles(2),
                Message = "Roles fetched successfully!"
            });
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO role)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<RoleResponseDTO>
                {
                    Data = await _roleService.AddRole(role),
                    Message = "Role added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-role")]
        public async Task<IActionResult> ModifyRole([FromBody] RoleModifyDTO role)
        {
            try
            {
                return Ok(new ResponseModel<RoleResponseDTO>
                {
                    Data = await _roleService.ModifyRole(role),
                    Message = "Role modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Role

        #region Action
        [HttpGet("get-all-actions")]
        public async Task<IActionResult> GetAllActions(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<ActionResponseDTO>>
            {
                Data = await _actionService.GetAllActions(status),
                Message = "Actions fetched successfully!"
            });
        }

        [HttpPost("add-action")]
        public async Task<IActionResult> AddAction([FromBody] ActionRequestDTO action)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<ActionResponseDTO>
                {
                    Data = await _actionService.AddAction(action),
                    Message = "Action added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-action")]
        public async Task<IActionResult> ModifyAction([FromBody] ActionModityDTO action)
        {
            try
            {
                return Ok(new ResponseModel<ActionResponseDTO>
                {
                    Data = await _actionService.ModifyAction(action),
                    Message = "Action modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Action

        #region Module
        [HttpGet("get-all-modules")]
        public async Task<IActionResult> GetAllModules(int status = 2)
        {
            return Ok(new ResponseModel<IEnumerable<ModuleResponseDTO>>
            {
                Data = await _moduleService.GetAllModules(status),
                Message = "Modules fetched successfully!"
            });
        }

        [HttpPost("add-module")]
        public async Task<IActionResult> AddModule([FromBody] ModuleRequestDTO module)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<ModuleResponseDTO>
                {
                    Data = await _moduleService.AddModule(module),
                    Message = "Module added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-module")]
        public async Task<IActionResult> ModifyModule([FromBody] ModuleModifyDTO module)
        {
            try
            {
                return Ok(new ResponseModel<ModuleResponseDTO>
                {
                    Data = await _moduleService.ModifyModule(module),
                    Message = "Module modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion Module

        #region UserRole
        [HttpGet("get-all-user-roles")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            return Ok(new ResponseModel<IEnumerable<UserRoleResponseDTO>>
            {
                Data = await _userRoleService.GetAllUserRoles(),
                Message = "User roles fetched successfully!"
            });
        }

        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleRequestDTO userRole)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<UserRoleResponseDTO>
                {
                    Data = await _userRoleService.AddUserRole(userRole),
                    Message = "User role added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-user-role")]
        public async Task<IActionResult> ModifyUserRole([FromBody] UserRoleModifyDTO userRole)
        {
            try
            {
                return Ok(new ResponseModel<UserRoleResponseDTO>
                {
                    Data = await _userRoleService.ModifyUserRole(userRole),
                    Message = "User role modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-user-role")]
        public async Task<IActionResult> RemoveUserRole(int id)
        {
            try
            {
                await _userRoleService.RemoveUserRole(id);

                return Ok(new ResponseModel<object>
                {
                    Message = "User role has been removed!"
                });
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion UserRole

        #region ModuleAction
        [HttpGet("get-all-module-actions")]
        public async Task<IActionResult> GetAllModuleActions()
        {
            return Ok(new ResponseModel<IEnumerable<ModuleActionResponseDTO>>
            {
                Data = await _moduleActionService.GetAllModuleActions(),
                Message = "Module actions fetched successfully!"
            });
        }

        [HttpPost("add-module-action")]
        public async Task<IActionResult> AddModuleAction([FromBody] ModuleActionRequestDTO moduleAction)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<ModuleActionResponseDTO>
                {
                    Data = await _moduleActionService.AddModuleAcction(moduleAction),
                    Message = "Module action added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-module-action")]
        public async Task<IActionResult> ModifyModuleAction([FromBody] ModuleActionModifyDTO moduleAction)
        {
            try
            {
                return Ok(new ResponseModel<ModuleActionResponseDTO>
                {
                    Data = await _moduleActionService.ModifyModuleAction(moduleAction),
                    Message = "Module action modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-module-action")]
        public async Task<IActionResult> ActionResult(int id)
        {
            try
            {
                await _moduleActionService.RemoveModuleAction(id);

                return Ok(new ResponseModel<object>
                {
                    Message = "Module action has been removed!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion ModuleAction

        #region ModuleAccess
        [HttpGet("get-all-module-access")]
        public async Task<IActionResult> GetAllModuleAccess()
        {
            return Ok(new ResponseModel<IEnumerable<ModuleAccessResponseDTO>>
            {
                Data = await _moduleAccessService.GetAllModuleAccess(),
                Message = "Module access fetched successfully!"
            });
        }

        [HttpPost("add-module-access")]
        public async Task<IActionResult> AddModuleAccess([FromBody] ModuleAccessRequestDTO moduleAccess)
        {
            try
            {
                return Created(string.Empty, new ResponseModel<ModuleAccessResponseDTO>
                {
                    Data = await _moduleAccessService.AddModuleAccess(moduleAccess),
                    Message = "Module access added successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("modify-module-access")]
        public async Task<IActionResult> ModifyModuleAccess([FromBody] ModuleAccessModifyDTO moduleAccess)
        {
            try
            {
                return Ok(new ResponseModel<ModuleAccessResponseDTO>
                {
                    Data = await _moduleAccessService.ModifyModuleAccess(moduleAccess),
                    Message = "Module access modified successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-module-access")]
        public async Task<IActionResult> RemoveModuleAccess(int id)
        {
            try
            {
                await _moduleAccessService.RemoveModuleAccess(id);

                return Ok(new ResponseModel<object>
                {
                    Message = "Module access has been removed!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion ModuleAccess
    }
}
