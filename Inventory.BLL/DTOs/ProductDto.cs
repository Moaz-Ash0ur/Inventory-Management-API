using Microsoft.AspNetCore.Http;

namespace Inventory.BLL.DTOs
{
    public class ProductDto : BaseDto
    {
        public string? SKU { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int ReorderLevel { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

    }

}
