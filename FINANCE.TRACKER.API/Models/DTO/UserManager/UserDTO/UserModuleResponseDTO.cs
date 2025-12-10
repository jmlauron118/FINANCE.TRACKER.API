namespace FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO
{
    public class UserModuleResponseDTO
    {
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ModulePage { get; set; }
        public string? Icon { get; set; }
        public int SortOrder { get; set; }
        public int ParentId { get; set; }
        public int ChildCount { get; set; }
    }
}
