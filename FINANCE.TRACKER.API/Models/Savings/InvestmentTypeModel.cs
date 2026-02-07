using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.Savings
{
    public class InvestmentTypeModel
    {
        [Key]
        public int InvestmentTypeId { get; set; }

        [Required]
        public string InvestmentTypeName { get; set; }

        [Required]
        public string Description { get; set; }

        public int IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated{ get; set; }
    }
}
