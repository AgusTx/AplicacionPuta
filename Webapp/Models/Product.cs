using System.ComponentModel.DataAnnotations;

namespace Webapp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}