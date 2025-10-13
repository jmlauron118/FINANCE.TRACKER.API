using FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleAccessDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleActionDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;
using FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(await _userService.GellAllUsers(status));
        }

        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                return Ok(await _userService.GetUserById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDTO user)
        {
            try
            {
                return Created(string.Empty, await _userService.AddUser(user));
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
                return Ok(await _userService.ModifyUser(user));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        #endregion User

        #region Role
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles(int status = 2)
        {
            return Ok(await _roleService.GetAllRoles(2));
        }

        [HttpGet("get-role-by-id")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                return Ok(await _roleService.GetRoleById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO role)
        {
            try
            {
                return Created(string.Empty, await _roleService.AddRole(role));
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
                return Ok(await _roleService.ModifyRole(role));
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
            return Ok(await _actionService.GetAllActions(status));
        }

        [HttpGet("get-action-by-id")]
        public async Task<IActionResult> GetActionById(int id)
        {
            try
            {
                return Ok(await _actionService.GetActionById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-action")]
        public async Task<IActionResult> AddAction([FromBody] ActionRequestDTO action)
        {
            try
            {
                return Created(string.Empty, await _actionService.AddAction(action));
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
                return Ok(await _actionService.ModifyAction(action));
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
            return Ok(await _moduleService.GetAllModules(status));
        }

        [HttpGet("get-module-by-id")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            try
            {
                return Ok(await _moduleService.GetModuleById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-module")]
        public async Task<IActionResult> AddModule([FromBody] ModuleRequestDTO module)
        {
            try
            {
                return Created(string.Empty, await _moduleService.AddModule(module));
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
                return Ok(await _moduleService.ModifyModule(module));
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
            return Ok(await _userRoleService.GetAllUserRoles());
        }

        [HttpGet("get-user-role-by-id")]
        public async Task<IActionResult> GetUserRoleById(int id)
        {
            try
            {
                return Ok(await _userRoleService.GetUserRoleById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleRequestDTO userRole)
        {
            try
            {
                return Created(string.Empty, await _userRoleService.AddUserRole(userRole));
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
                return Ok(await _userRoleService.ModifyUserRole(userRole));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        #endregion UserRole

        #region ModuleAction
        [HttpGet("get-all-module-actions")]
        public async Task<IActionResult> GetAllModuleActions()
        {
            return Ok(await _moduleActionService.GetAllModuleActions());
        }

        [HttpGet("get-module-action-by-id")]
        public async Task<IActionResult> GetModuleActionById(int id)
        {
            try
            {
                return Ok(await _moduleActionService.GetModuleActionById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-module-action")]
        public async Task<IActionResult> AddModuleAction([FromBody] ModuleActionRequestDTO moduleAction)
        {
            try
            {
                return Created(string.Empty, await _moduleActionService.AddModuleAcction(moduleAction));
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
                return Ok(await _moduleActionService.ModifyModuleAction(moduleAction));
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

                return NoContent();
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
            return Ok(await _moduleAccessService.GetAllModuleAccess());
        }

        [HttpGet("get-module-access-by-id")]
        public async Task<IActionResult> GetModuleAccessById(int id)
        {
            try
            {
                return Ok(await _moduleAccessService.GetModuleAccessById(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-module-access")]
        public async Task<IActionResult> AddModuleAccess([FromBody] ModuleAccessRequestDTO moduleAccess)
        {
            try
            {
                return Created(string.Empty, await _moduleAccessService.AddModuleAccess(moduleAccess));
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
                return Ok(await _moduleAccessService.ModifyModuleAccess(moduleAccess));
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

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion ModuleAccess
    }
}
