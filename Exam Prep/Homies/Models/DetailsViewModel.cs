using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Homies.Models
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 15)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        public string Organiser { get; set; } = null!;
        public string Type { get; set; }
        public int TypeId { get; set; }
        public ICollection<TypesViewModel> Types { get; set; } = new List<TypesViewModel>();
    }
}
