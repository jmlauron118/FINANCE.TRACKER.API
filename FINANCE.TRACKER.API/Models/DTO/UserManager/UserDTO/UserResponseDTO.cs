namespace FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }

        public string ? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Gender { get; set; }

        public string? Username { get; set; }

        public int IsActive { get; set; }
    }
}
