using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Homies.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 15)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string OrganiserId { get; set; } = string.Empty;

        [ForeignKey("OrganiserId")] 
        public IdentityUser Organiser { get; set; } = null!;

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        [Required]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public Type Type { get; set; }

        public ICollection<EventParticipant> EventsParticipants { get; set; } = new List<EventParticipant>();
    }
}
