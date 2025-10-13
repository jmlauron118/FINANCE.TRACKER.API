using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "First name is required!")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public int IsActive { get; set; }
    }
}
