using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface ISupplierService : IBaseService<Supplier, SupplierDto>
    {
        // مثال: إرجاع الموردين الأكثر توريداً
        //IEnumerable<SupplierDto> GetTopSuppliers(int top);
       // IEnumerable<SupplierDto> Search(string keyword);
        SupplierDto? GetByPhone(string phone);
        SupplierDto? GetByEmail(string email);
    }





}






