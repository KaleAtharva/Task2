using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class Production
    {
        [Required]
        public string item { get; set; }
        [Required]

        public int quantity { get; set; }
    }
}