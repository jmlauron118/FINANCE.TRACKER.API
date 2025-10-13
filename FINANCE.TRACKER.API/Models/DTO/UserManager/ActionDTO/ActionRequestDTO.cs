using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO
{
    public class ActionRequestDTO
    {
        [Required(ErrorMessage = "Action name is required!")]
        public string? ActionName { get; set; }

        [Required(ErrorMessage = "Action description is required!")]
        public string? Description { get; set; }

        public int IsActive { get; set; }
    }
}
