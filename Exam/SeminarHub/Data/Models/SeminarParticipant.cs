using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SeminarHub.Data.Models
{
    [Comment("Seminar Participants mapping table")]
    public class SeminarParticipant
    {
        [Key]
        [Comment("Seminar Identifier")]
        public int SeminarId { get; set; }

        [ForeignKey(nameof(SeminarId))]
        [Comment("Foreign key of the Seminar")]
        public Seminar Seminar { get; set; } = null!;

        [Key]
        [Comment("Participant Identifier")]
        public string ParticipantId { get; set; } = string.Empty;

        [ForeignKey(nameof(ParticipantId))]
        [Comment("Foreign key of the participant")]
        public IdentityUser Participant { get; set; } = null!;
    }
}
