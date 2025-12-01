using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public partial class StockTransactionService : BaseService<StockTransaction, StockTransactionDto>, IStockTransactions
    {
        private readonly IGenericRepository<StockTransaction> _stockRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public StockTransactionService(
            IGenericRepository<StockTransaction> repo,
            IMapper mapper,
            IUserService userService
,
            IGenericRepository<Product> productRepo) : base(repo, mapper, userService)
        {
            _stockRepo = repo;
            _mapper = mapper;
            _userService = userService;
            _productRepo = productRepo;
        }

        public int GetCurrentStock(int productId)
        {
            return _stockRepo
                .GetList(s => s.ProductId == productId)
                .Sum(s => s.ChangeQty);
        }

        public IEnumerable<StockTransactionDto> GetProductTransactions(int productId)
        {
            var transactions = _stockRepo
                .GetList(s => s.ProductId == productId)
                .OrderByDescending(s => s.CreatedDate);
               
            return _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
        }

        public bool IsBelowReorderLevel(int productId)
        {
            var product = _productRepo.GetByID(productId);
            if (product == null) return false;

            int currentStock = GetCurrentStock(productId);
            return currentStock <= product.ReorderLevel;
        }

        public IEnumerable<StockBalanceDto> GetStockBalances()
        {
            var query = _stockRepo.GetAll()
                .GroupBy(s => s.ProductId)
                .Select(g => new StockBalanceDto
                {
                    ProductId = g.Key,
                    Balance = g.Sum(x => x.ChangeQty)
                })
                .ToList();

            return query;
        }


    }

}

