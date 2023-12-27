using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class Store
    {
        [Required]
        public int itemID { get; set; }
        [Required]
        public string item { get; set; }
        [Required]

        public bool available { get; set; }
        [Required]
    
        public int quantity { get; set; }
        
    }
}