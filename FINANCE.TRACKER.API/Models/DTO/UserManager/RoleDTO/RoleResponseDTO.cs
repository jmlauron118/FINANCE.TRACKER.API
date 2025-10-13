namespace FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO
{
    public class RoleResponseDTO
    {
        public int RoleId { get; set; }
        public string? Role { get; set; }
        public string? Description { get; set; }
        public int IsActive { get; set; }
    }
}
