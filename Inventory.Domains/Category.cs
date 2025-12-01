using System.ComponentModel.DataAnnotations;

namespace Inventory.Domains
{
    public class Category : BaseTable
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }





}
