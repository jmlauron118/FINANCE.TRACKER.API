using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO
{
    public class ModuleRequestDTO
    {
        [Required(ErrorMessage = "Module name is required!")]
        public string? ModuleName { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Module page is required!")]
        public string? ModulePage { get; set; }

        [Required(ErrorMessage = "Module code is required!")]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "Sort order is required!")]
        public int SortOrder { get; set; }

        public int IsActive { get; set; }
    }
}
