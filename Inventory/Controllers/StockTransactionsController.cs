using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services;
using InventoryApi.Helper;
using static Inventory.BLL.Services.StockTransactionService;

namespace InventoryApi.Controllers
{
    [Authorize(Roles = $"{SystemRoles.Admin},{SystemRoles.InventoryManager}")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionsController : ControllerBase
    {
        private readonly IStockTransactions _stockService;

        public StockTransactionsController(IStockTransactions stockService)
        {
            _stockService = stockService;
        }

        // 📌 Check if product is below reorder level
        [HttpGet("IsBelowReorderLevel/{productId:int}")]
        public IActionResult IsBelowReorderLevel(int productId)
        {
            var result = _stockService.IsBelowReorderLevel(productId);
            return Ok(ApiResponse<bool>.SuccessResponse(result, $"Product {(result ? "is" : "is not")} below reorder level."));
        }


        // 📌 Get current stock for a product
        [HttpGet("CurrentStock/{productId:int}")]
        public IActionResult GetCurrentStock(int productId)
        {
            var result = _stockService.GetCurrentStock(productId);
            return Ok(ApiResponse<int>.SuccessResponse(result, "Current stock fetched successfully."));
        }


        // 📌 Get transactions for a specific product
        [HttpGet("ProductTransactions/{productId:int}")]
        public IActionResult GetProductTransactions(int productId)
        {
            var result = _stockService.GetProductTransactions(productId);
            return Ok(ApiResponse<IEnumerable<StockTransactionDto>>.SuccessResponse(result, "Product transactions fetched successfully."));
        }


        // 📌 Get stock balances for all products
        [HttpGet("StockBalances")]
        public IActionResult GetStockBalances()
        {
            var result = _stockService.GetStockBalances();
            return Ok(ApiResponse<IEnumerable<StockBalanceDto>>.SuccessResponse(result, "Stock balances fetched successfully."));
        }
    
    
    }

}
