using System.ComponentModel.DataAnnotations;

namespace TextSplitterApp.Models
{
    public class TextViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Text { get; set; } = null!;

        public string SplitText { get; set; } = null!;

    }
}
