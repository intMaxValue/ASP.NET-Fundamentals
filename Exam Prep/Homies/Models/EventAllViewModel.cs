using Type = Homies.Data.Models.Type;

namespace Homies.Models
{
    public class EventAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Start { get; set; }

        public string Type { get; set; }

        public string Organiser { get; set; }
    }
}
