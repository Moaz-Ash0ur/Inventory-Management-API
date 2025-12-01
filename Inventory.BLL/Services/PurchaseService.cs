using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.DAL.Repositories;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class PurchaseService : BaseService<Purchase, PurchaseDto>, IPurchaseServices
    {
        private readonly IGenericRepository<Purchase> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseService(IGenericRepository<Purchase> repo,IMapper mapper,
            IUserService userService,IUnitOfWork unitOfWork) : 
            base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }

        public PurchaseDto GetByInvoice(string invoiceNo)
        {
            var purchases = _repo.GetFirst(p => p.InvoiceNo == invoiceNo);
            return _mapper.Map<PurchaseDto>(purchases);
        }
         
        public IEnumerable<PurchaseDto> GetPurchasesBySupplier(int supplierId)
        {
            var purchases = _repo.GetList(p => p.SupplierId == supplierId);
            return _mapper.Map<IEnumerable<PurchaseDto>>(purchases);
        }

        public IEnumerable<PurchaseDto> GetPurchasesByDateRange(DateTime from, DateTime to)
        {
            var purchases = _repo.GetList(p => p.PurchaseDate >= from && p.PurchaseDate <= to);
            return _mapper.Map<IEnumerable<PurchaseDto>>(purchases);
        }


        public async Task AddPurchaseWithStock(PurchaseDto purchaseDto)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                var purchase = AddPurchase(purchaseDto);

                foreach (var item in purchaseDto.Items)
                {
                     AddPurchaseItem(purchase.Id, item);
                     AddStockTransaction(purchase.Id, item);
                }

                await _unitOfWork.CommitChanges();
            }
            catch
            {
                await _unitOfWork.Rollback();
               
            }
        }


       //Refactor Func   
        private Purchase AddPurchase(PurchaseDto dto)
        {
            var repo = _unitOfWork.GetRepository<Purchase>();

            dto.TotalAmount = (decimal)dto.Items.Sum(p => p.TotalCost)!;

            if (this.Insert(dto,out int purchaseId))
            {
               return repo.GetByID(purchaseId);
            }

            return null;
        }

        private void AddPurchaseItem(int purchaseId, PurchaseItemDto dto)
        {
            var repo = _unitOfWork.GetRepository<PurchaseItem>();
            var purchaseItem = new PurchaseItem
            {
                PurchaseId = purchaseId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitCost = dto.UnitCost,
                TotalCost = dto.Quantity * dto.UnitCost
            };
             var s =  repo.Insert(purchaseItem);
        }

        private void AddStockTransaction(int purchaseId, PurchaseItemDto dto)
        {
            var repo = _unitOfWork.GetRepository<StockTransaction>();
            var stockTransaction = new StockTransaction
            {
                ProductId = dto.ProductId,
                CreatedDate = DateTime.Now,
                ChangeQty = dto.Quantity,
                Type = "Purchase",
                ReferenceId = purchaseId
            };
            var s =  repo.Insert(stockTransaction);
        }

        public IEnumerable<PurchaseDto> GetPurchaseDetails(string invoiceNo)
        {
            var PurchaseDetails = _repo
                .GetAllQueryable()
                .Where(s => s.InvoiceNo == invoiceNo)                                      
                .Include(s => s.PurchaseItems)
                .AsNoTracking();

            return _mapper.Map<IEnumerable<PurchaseDto>>(PurchaseDetails);
        }


       

    }


}
