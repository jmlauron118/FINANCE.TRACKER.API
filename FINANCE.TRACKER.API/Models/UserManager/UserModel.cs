using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.UserManager
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string? Firstname { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Lastname { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Gender { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Username { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Password { get; set; }

        public int IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
