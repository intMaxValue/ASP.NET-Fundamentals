using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Models
{
    public class EditSeminarViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(SeminarTopicMaxLength, MinimumLength = SeminarTopicMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarLecturerMaxLength, MinimumLength = SeminarLecturerMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarDetailsMaxLength, MinimumLength = SeminarDetailsMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Details { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateAndTime { get; set; }

        public string OrganizerId { get; set; } = string.Empty;

        [Range(SeminarDurationMinLength, SeminarDurationMaxLength)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public ICollection<CategoriesViewModel> Categories { get; set; } = new List<CategoriesViewModel>();
    }
}
