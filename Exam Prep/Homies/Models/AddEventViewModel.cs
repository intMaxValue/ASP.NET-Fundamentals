using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Type = Homies.Data.Models.Type;

namespace Homies.Models
{
    public class AddEventViewModel
    {

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

        public Type? Type { get; set; }
        public int TypeId { get; set; }
        public ICollection<TypesViewModel> Types { get; set; } = new List<TypesViewModel>();
    }
}
