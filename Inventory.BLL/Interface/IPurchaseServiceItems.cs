using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface IPurchaseServiceItems : IBaseService<PurchaseItem, PurchaseItemDto>
    {
        // ممكن تضيف دوال إضافية مثل: إجمالي تكلفة منتج معين
        //     decimal GetTotalCostByProduct(int productId);
    }





}






