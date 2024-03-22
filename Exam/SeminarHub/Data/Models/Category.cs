using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Models
{
    [Comment("Category model")]
    public class Category
    {
        [Key]
        [Comment("Category Identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength,
            ErrorMessage = LengthErrorMessage)]
        [Comment("Category Name")]
        public string Name { get; set; } = string.Empty;

        [Comment("Collection of seminars")]
        public ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
