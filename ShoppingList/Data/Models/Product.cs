using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Data.Models
{
    [Comment("Shopping List Product")]
    public class Product
    {
        [Key]
        [Comment("Product Identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public List<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();
    }
}
