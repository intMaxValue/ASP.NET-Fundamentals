using System.ComponentModel.DataAnnotations;

namespace Homies.Data.Models
{
    public class Type
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
