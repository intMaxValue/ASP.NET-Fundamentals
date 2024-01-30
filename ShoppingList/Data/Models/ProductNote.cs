using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Data.Models
{
    [Comment("Product Note")]
    public class ProductNote
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(150)]
        [Comment("Note Content")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Comment("Product Identifier")]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
