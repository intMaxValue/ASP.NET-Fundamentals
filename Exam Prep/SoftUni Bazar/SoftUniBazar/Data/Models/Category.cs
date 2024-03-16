using System.ComponentModel.DataAnnotations;

using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    }
}
