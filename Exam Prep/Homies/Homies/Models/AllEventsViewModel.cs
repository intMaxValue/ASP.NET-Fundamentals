using System.ComponentModel.DataAnnotations;

using static Homies.Data.DataConstants;

namespace Homies.Models
{
    public class AllEventsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Organiser { get; set; } = string.Empty;
    }
}
