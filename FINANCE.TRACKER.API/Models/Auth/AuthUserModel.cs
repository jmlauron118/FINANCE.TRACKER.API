using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;

namespace FINANCE.TRACKER.API.Models.Auth
{
    public class AuthUserModel
    {
        public int UserId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public IEnumerable<UserModuleResponseDTO>? UserModules { get; set; }
    }
}
