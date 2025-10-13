using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.UserManager
{
    public class UserRoleModel
    {
        [Key]
        public int UserRoleId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public int CreatedBy { get; set; } 


        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
