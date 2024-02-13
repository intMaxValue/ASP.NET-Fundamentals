using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Data.Models
{
    public class EventParticipant
    {
        [Key]
        public string HelperId { get; set; }

        [ForeignKey("HelperId")]
        public IdentityUser Helper { get; set; }

        [Key]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
