using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface IPurchaseServices : IBaseService<Purchase, PurchaseDto>
    {
        IEnumerable<PurchaseDto> GetPurchaseDetails(string invoiceNo);
        PurchaseDto GetByInvoice(string invoiceNo);
        IEnumerable<PurchaseDto> GetPurchasesBySupplier(int supplierId);
        IEnumerable<PurchaseDto> GetPurchasesByDateRange(DateTime from, DateTime to);
        Task AddPurchaseWithStock(PurchaseDto purchaseDto);
   
    }






}






