using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface IProductService : IBaseService<Product, ProductDto>
    {
        public int GetCurrentStock(int productId);
        IEnumerable<ProductDto> GetMostSoldProducts(int top = 5);
        public IEnumerable<ProductDto> GetLowStockProducts();
        public ProductDto GetBySKU(string sku);



    }



}






