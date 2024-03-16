using SoftUniBazar.Data.Models;

using System.ComponentModel.DataAnnotations;
using SoftUniBazar.Data;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Models
{
    public class AddNewAdViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(AdNameMaxLength, MinimumLength = AdNameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [DisplayFormat(DataFormatString = DataConstants.DateTimeFormat)]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(AdDescriptionMaxLength, MinimumLength = AdDescriptionMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }
        
        public ICollection<CategoriesViewModel> Categories { get; set; } = new List<CategoriesViewModel>();
    }
}
