namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO
{
    public class ActionResponseDTO
    {
        public int ActionId { get; set; }

        public string? ActionName { get; set; }

        public string? Description { get; set; }

        public int IsActive { get; set; }
    }
}
