namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleActionDTO
{
    public class ModuleActionResponseDTO
    {
        public int ModuleActionId { get; set; }

        public int ModuleId { get; set; }

        public string? ModuleName { get; set; }

        public string? ModuleDescription { get; set; }

        public int ActionId { get; set; }

        public string? ActionName { get; set; }

        public string? ActionDescription { get; set; }

    }
}
