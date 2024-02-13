using System.ComponentModel.DataAnnotations;

namespace Homies.Models
{
    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 15)]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public string Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public string End { get; set; }

        public int TypeId { get; set; }

        public ICollection<TypesViewModel> Types { get; set; } = new List<TypesViewModel>();
    }
}
