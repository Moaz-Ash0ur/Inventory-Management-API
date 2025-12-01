using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.Domains;

namespace Inventory.BLL.Services
{
    public class PurchaseItemService : BaseService<PurchaseItem, PurchaseItemDto>, IPurchaseServiceItems
    {
        private readonly IGenericRepository<PurchaseItem> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PurchaseItemService(
            IGenericRepository<PurchaseItem> repo,
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
