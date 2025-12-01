using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface ISaleItems : IBaseService<SaleItem, SaleItemDto>
    {
        // مثال: جلب كل العناصر الخاصة بفاتورة بيع
      //  IEnumerable<SaleItemDto> GetBySaleId(int saleId);
    }





}






