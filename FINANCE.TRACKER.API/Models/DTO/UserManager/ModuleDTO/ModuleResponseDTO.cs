namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO
{
    public class ModuleResponseDTO
    {
        public int ModuleId { get; set; }

        public string? ModuleName { get; set; }

        public string? Description { get; set; }

        public string? ModulePage { get; set; }

        public string? Icon { get; set; }

        public int SortOrder { get; set; }  

        public int IsActive { get; set; }
    }
}
