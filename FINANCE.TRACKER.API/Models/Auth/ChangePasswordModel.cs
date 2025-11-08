using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.Auth
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
