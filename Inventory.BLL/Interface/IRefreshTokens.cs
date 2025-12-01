using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.DAL.Models;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface IRefreshTokens : IBaseService<RefreshToken, RefreshTokenDto>
    {
        public bool Refresh(RefreshTokenDto tokenDto);
        public bool IsExpireToken(string token);
        public RefreshTokenDto GetByToken(string token);

    }





}






