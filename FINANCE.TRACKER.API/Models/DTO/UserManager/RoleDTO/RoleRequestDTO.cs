using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO
{
    public class RoleRequestDTO
    {
        [Required(ErrorMessage = "Role name is required!")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Role description is required!")]
        public string? Description { get; set; }

        public int IsActive { get; set; }
    }
}
