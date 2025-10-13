namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleAccessDTO
{
    public class ModuleAccessResponseDTO
    {
        public int ModuleAccessId { get; set; }
        public int ModuleActionId { get; set; }
        public string? ModuleName { get; set; }
        public string? ActionName { get; set; }
        public int UserRoleId { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}
