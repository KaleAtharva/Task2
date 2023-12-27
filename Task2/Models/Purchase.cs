using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class Purchase
    {
        [Required]
        public int itemID { get; set; }
        [Required]
        public string item { get; set; }
        [Required]

        public int quantity { get; set; }
    }
}