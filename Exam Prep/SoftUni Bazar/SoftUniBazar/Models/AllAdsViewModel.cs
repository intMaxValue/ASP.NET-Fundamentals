using SoftUniBazar.Data.Models;

namespace SoftUniBazar.Models
{
    public class AllAdsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Owner { get; set; } = null!;
        
    }
}
