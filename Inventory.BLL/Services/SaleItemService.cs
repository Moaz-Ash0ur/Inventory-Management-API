using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class SaleItemService : BaseService<SaleItem, SaleItemDto>, ISaleItems
    {
        private readonly IGenericRepository<SaleItem> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SaleItemService(
            IGenericRepository<SaleItem> repo,
            IMapper mapper,
            IUserService userService
        ) : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }
    }

}

