using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BLL.Interface.Base
{
    public interface IBaseService<T, DTO>
    {
        IEnumerable<DTO> GetAll();
        IQueryable<DTO> GetAllQueryable();
        DTO GetByID(int Id);
        bool Insert(DTO entity);
        bool Insert(DTO entity,out int Id);
        bool ChangeStatus(int ID, string userId="");
        bool Update(DTO entity);
    }
}
