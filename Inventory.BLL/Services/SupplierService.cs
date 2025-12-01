using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class SupplierService : BaseService<Supplier, SupplierDto>, ISupplierService
    {
        private readonly IGenericRepository<Supplier> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SupplierService(
            IGenericRepository<Supplier> repo,
            IMapper mapper,
            IUserService userService
        ) : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }
     
        public SupplierDto GetByPhone(string phone)
        {
            var supplier = _repo.GetFirst(s => s.Phone == phone);
            return _mapper.Map<SupplierDto>(supplier);
        }

        public SupplierDto GetByEmail(string email)
        {
            var supplier = _repo.GetFirst(s => s.Email == email);
            return _mapper.Map<SupplierDto>(supplier);
        }



    }


}
