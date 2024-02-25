using System.ComponentModel.DataAnnotations;

using static Homies.Data.DataConstants;

namespace Homies.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(EventNameMaxLength, MinimumLength = EventNameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(EventDescriptionMaxLength, MinimumLength = EventDescriptionMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TypeId { get; set; }

        public ICollection<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
