using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.Domains;
using static Inventory.BLL.Services.StockTransactionService;

namespace Inventory.BLL.Interface
{
    public interface IStockTransactions : IBaseService<StockTransaction, StockTransactionDto>
    {
        bool IsBelowReorderLevel(int productId);

        int GetCurrentStock(int productId);

        IEnumerable<StockTransactionDto> GetProductTransactions(int productId);

        IEnumerable<StockBalanceDto> GetStockBalances();


    }





}






