using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class SaleService : BaseService<Sale, SaleDto>, ISalesService
    {
        private readonly IGenericRepository<Sale> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISaleItems _saleItems;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockTransactions _stockTransactions;


        public SaleService(IGenericRepository<Sale> repo, IMapper mapper,
            IUserService userService, ISaleItems saleItems, IUnitOfWork unitOfWork, IStockTransactions stockTransactions) : base(unitOfWork, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
            _saleItems = saleItems;
            _unitOfWork = unitOfWork;
            _stockTransactions = stockTransactions;
        }


        public async Task AddSaleWithStockAsyn(SaleDto saleDto)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                var sale = AddSale(saleDto);

                foreach (var item in saleDto.SaleItems)
                {
                     AddSaleItem(sale.Id, item);
                     AddStockTransaction(sale.Id, item);
                }
                await _unitOfWork.CommitChanges();
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }


        private Sale AddSale(SaleDto dto)
        {
            var repo = _unitOfWork.GetRepository<Sale>();

            if (this.Insert(dto, out int purchaseId))
            {
                return repo.GetByID(purchaseId);
            }

            return null;
        }

        private void AddSaleItem(int saleId, SaleItemDto dto)
        {     
            var repo = _unitOfWork.GetRepository<SaleItem>();
            var saleItem = _mapper.Map<SaleItem>(dto);
            saleItem.SaleId = saleId;
             repo.Insert(saleItem);
        }

        private void AddStockTransaction(int saleId, SaleItemDto dto)
        {

            var repo = _unitOfWork.GetRepository<StockTransaction>();
        
            var stockTransaction = new StockTransaction
            {
                ProductId = dto.ProductId,
                CreatedDate = DateTime.Now,                
                ChangeQty = -dto.Quantity,
                Type = "Sale",
                ReferenceId = saleId
            };
              repo.Insert(stockTransaction);

        }

    
        public IEnumerable<SaleDto> GetByDateRange(DateTime from, DateTime to)
        {
            var purchases = _repo.GetList(p => p.SaleDate >= from && p.SaleDate <= to);
            return _mapper.Map<IEnumerable<SaleDto>>(purchases);
        }


        public IEnumerable<SaleDto> GetSalesByCustomer(int customerId)
        {
            var sales = _repo.GetList(s => s.CustomerId == customerId);             
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }


        public SaleDto GetByInvoice(string invoiceNo)
        {
            var sale = _repo.GetFirst(s => s.InvoiceNo == invoiceNo);
            return _mapper.Map<SaleDto>(sale);
        }
      

        public IEnumerable<MostOrderedProductsDto> GetTopSellingProducts(int top)
            {
                var query = _saleItems.GetAll()
                    .GroupBy(s => s.ProductId)
                    .Select(g => new MostOrderedProductsDto
                    {
                        ProductId = g.Key,
                        TotalQuantity = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(top)
                    .ToList();

                return query;
            }

        public IEnumerable<SaleDto> GetSalesDetails(string invoiceNo)
        {
            var salesDetails = _repo.GetAllQueryable()
                 .AsNoTracking()
                 .Include(s => s.SaleItems)
                 .Where(s => s.InvoiceNo == invoiceNo);
                 
                
            return _mapper.Map<IEnumerable<SaleDto>>(salesDetails);
        }

   
    }

}

