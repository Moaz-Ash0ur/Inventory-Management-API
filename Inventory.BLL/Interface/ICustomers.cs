using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface ICustomerService : IBaseService<Customer, CustomerDto>
    {
        public IEnumerable<CustomerDto> GetTopCustomers(int top = 3);

        public CustomerDto GetByPhone(string phone);

        public CustomerDto GetByEmail(string email);

    }





}






