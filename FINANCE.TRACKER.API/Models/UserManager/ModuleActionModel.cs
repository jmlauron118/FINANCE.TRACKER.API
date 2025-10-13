using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.UserManager
{
    public class ModuleActionModel
    {
        [Key]
        public int ModuleActionId { get; set; }

        public int ModuleId { get; set; }

        public int ActionId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
