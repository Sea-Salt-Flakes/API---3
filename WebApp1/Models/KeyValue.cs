using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class KeyValueItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Value { get; set; } = string.Empty;

        // optional extra field
        [MaxLength(250)]
        public string? Description { get; set; }
    }
}