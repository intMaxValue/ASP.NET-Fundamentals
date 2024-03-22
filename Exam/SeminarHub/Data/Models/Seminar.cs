using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Models
{
    [Comment("Seminar model")]
    public class Seminar
    {
        [Key]
        [Comment("Seminar Identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(SeminarTopicMaxLength, MinimumLength = SeminarTopicMinLength,
            ErrorMessage = LengthErrorMessage)]
        [Comment("Topic of the seminar")]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarLecturerMaxLength, MinimumLength = SeminarLecturerMinLength,
            ErrorMessage = LengthErrorMessage)]
        [Comment("Lecturer of the seminar")]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarDetailsMaxLength, MinimumLength = SeminarDetailsMinLength,
            ErrorMessage = LengthErrorMessage)]
        [Comment("Details about the seminar")]
        public string Details { get; set; } = string.Empty;

        [Required]
        [Comment("Organizer's Identifier")]
        public string OrganizerId { get; set; } = string.Empty;

        [ForeignKey(nameof(OrganizerId))]
        [Comment("Foreign key of the Organizer")]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = DateTimeFormat, ApplyFormatInEditMode = true)]
        [Comment("Date and time of the seminar")]
        public DateTime DateAndTime { get; set; }

        [Range(SeminarDurationMinLength, SeminarDurationMaxLength)]
        [Comment("Duration of the seminar")]
        public int Duration { get; set; }

        [Required]
        [Comment("Category Identifier")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [Comment("Foreign key of the Category")]
        public Category Category { get; set; } = null!;

        [Comment("Collection of the seminar's participants")]
        public ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
