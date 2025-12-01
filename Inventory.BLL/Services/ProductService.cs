using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{

    public class ProductService : BaseService<Product, ProductDto>, IProductService
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<StockTransaction> _stockRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ProductService(IGenericRepository<Product> repo,IMapper mapper,
            IUserService userService,IGenericRepository<StockTransaction> stockRepo) : 
            base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
            _stockRepo = stockRepo;
        }

        public int GetCurrentStock(int productId)
        {
            return _stockRepo.GetList(t => t.ProductId == productId)
                .Sum(t => t.ChangeQty);
        }

        public IEnumerable<ProductDto> GetLowStockProducts()
        {
            var query = _repo.GetAllQueryable()
                             .Where(p => p.StockTransactions
                             .Sum(t => t.ChangeQty) <= p.ReorderLevel)
                             .ToList();

            return _mapper.Map<IEnumerable<ProductDto>>(query);
        }
 
        public ProductDto GetBySKU(string sku)
        {
            var product = _repo.GetFirst(p => p.SKU == sku);
            return _mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> GetMostSoldProducts(int top = 5)
        {
            return new List<ProductDto>();
        }



    }


}
