namespace FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO
{
    public class UserRoleResponseDTO
    {
        public int UserRoleId { get; set; }

        public int UserId { get; set; }

        public string? Fullname { get; set; }

        public string? Username { get; set; }

        public int RoleId { get; set; }

        public string? Role { get; set; }
    }
}
