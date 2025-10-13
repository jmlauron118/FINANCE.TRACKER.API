using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.UserManager
{
    public class ActionModel
    {
        [Key]
        public int ActionId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? ActionName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? Description { get; set; }

        public int IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
