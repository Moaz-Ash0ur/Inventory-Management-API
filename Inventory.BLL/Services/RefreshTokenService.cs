using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services.Base;
using Inventory.DAL.Contracts;
using Inventory.DAL.Models;
using Inventory.Domains;
using static Inventory.BLL.Services.CustomerService;

namespace Inventory.BLL.Services
{
    public class RefreshTokenService : BaseService<RefreshToken, RefreshTokenDto>, IRefreshTokens
    {

        private IGenericRepository<RefreshToken> _repo;
        private IMapper _mapper;
        private readonly IUserService _userService;

        public RefreshTokenService(IGenericRepository<RefreshToken> repo, IMapper mapper,
            IUserService userService) : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }


        public bool Refresh(RefreshTokenDto tokenDto)
        {
            Guid userId = Guid.Parse(tokenDto.UserId);
            var allTokens = _repo.GetList(r => r.UserId == userId && r.CurrentState == 0);

            foreach (var dbToken in allTokens)
            {
                _repo.ChangeStatus(dbToken.Id, tokenDto.UserId);
            }
            this.Insert(tokenDto);
            return true;

            //var newToken = _mapper.Map<RefreshToken>(tokenDto);
            //newToken.CreatedBy = _userService.GetLoggedInUser();
          
        }

        public bool IsExpireToken(string token)
        {
            var storedToken = GetByToken(token);

            if (storedToken == null || storedToken.CurrentState == 1 || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        public RefreshTokenDto GetByToken(string token)
        {
            var myToken = _repo.GetFirst(r => r.Token == token);
            return _mapper.Map<RefreshTokenDto>(myToken);
        }


    }

}

