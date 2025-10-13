using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.UserManager
{
    public class RoleModel
    {
        [Key]
        public int RoleId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Role { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? Description { get; set; }

        public int IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
