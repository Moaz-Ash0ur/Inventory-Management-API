using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface ISalesService : IBaseService<Sale, SaleDto>
    {
        IEnumerable<SaleDto> GetSalesDetails(string invoiceNo);
        IEnumerable<SaleDto> GetByDateRange(DateTime from, DateTime to);
        Task AddSaleWithStockAsyn(SaleDto saleDto);
        IEnumerable<SaleDto> GetSalesByCustomer(int customerId);
        SaleDto GetByInvoice(string invoiceNo);
        IEnumerable<MostOrderedProductsDto> GetTopSellingProducts(int top);

    }





}






