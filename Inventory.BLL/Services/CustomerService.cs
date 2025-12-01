using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class CustomerService : BaseService<Customer, CustomerDto>, ICustomerService
    {
        private readonly IGenericRepository<Customer> _repo;
        private readonly IGenericRepository<Sale> _saleRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CustomerService(IGenericRepository<Customer> repo,IMapper mapper,
            IUserService userService,IGenericRepository<Sale> saleRepo) 
            : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
            _saleRepo = saleRepo;
        }

        public IEnumerable<CustomerDto> GetTopCustomers(int top = 3)
        {
            var query = _saleRepo.GetAll()
                .GroupBy(s => s.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalAmount = g.Sum(s => s.SubTotal)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(top)
                .ToList();

            var customers = _repo
            .GetList(c => query.Select(x => x.CustomerId)
            .Contains(c.Id));

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public CustomerDto GetByPhone(string phone)
        {
            var customer = _repo.GetFirst(c => c.Phone == phone);
            return _mapper.Map<CustomerDto>(customer);
        }

        public CustomerDto GetByEmail(string email)
        {
            var customers = _repo.GetFirst(c => c.Email == email);
            return _mapper.Map<CustomerDto>(customers);
        }

    }


}
