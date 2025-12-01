using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.Domains;
using InventoryApi.Helper;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _saleService;
        private readonly IStockTransactions _stockTransactions;

        public SalesController(ISalesService saleService, IStockTransactions stockTransactions)
        {
            _saleService = saleService;
            _stockTransactions = stockTransactions;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _saleService.GetAll();
            return Ok(ApiResponse<IEnumerable<SaleDto>>.SuccessResponse(result, "Sales fetched successfully."));
        }


        [HttpGet("Details")]
        public IActionResult GetSalesDetails(string invoiceNo)
        {
            var result = _saleService.GetSalesDetails(invoiceNo);
            return Ok(ApiResponse<IEnumerable<SaleDto>>.SuccessResponse(result, "Sales fetched successfully."));
        }


        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _saleService.GetByID(id);
            if (result == null)
                return NotFound(ApiResponse<SaleDto>.FailResponse("Sale not found"));

            return Ok(ApiResponse<SaleDto>.SuccessResponse(result, "Sale fetched successfully."));
        }


        [HttpGet("ByInvoice/{invoiceNo}")]
        public IActionResult GetByInvoice(string invoiceNo)
        {
            var result = _saleService.GetByInvoice(invoiceNo);
            if (result == null)
                return NotFound(ApiResponse<SaleDto>.FailResponse("Sale not found with given invoice"));

            return Ok(ApiResponse<SaleDto>.SuccessResponse(result, "Sale fetched successfully."));
        }


        [HttpGet("ByCustomer/{customerId}")]
        public IActionResult GetSalesByCustomer(int customerId)
        {
            var result = _saleService.GetSalesByCustomer(customerId);
            return Ok(ApiResponse<IEnumerable<SaleDto>>.SuccessResponse(result, "Sales by customer fetched successfully."));
        }


        [HttpGet("ByDateRange")]
        public IActionResult GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from == default || to == default)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid date range"));

            var result = _saleService.GetByDateRange(from, to);
            return Ok(ApiResponse<IEnumerable<SaleDto>>.SuccessResponse(result, "Sales in date range fetched successfully."));
        }


        [HttpGet("TopSellingProducts")]
        public IActionResult GetTopSellingProducts([FromQuery] int top = 3)
        {
            var result = _saleService.GetTopSellingProducts(top);
            return Ok(ApiResponse<IEnumerable<MostOrderedProductsDto>>.SuccessResponse(result, $"Top {top} selling products fetched successfully."));
        }



        [HttpPost]
        //need validation for when sales and check of the stock qty
        public async Task<IActionResult> AddSaleWithStock([FromBody] SaleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid sale data"));

            try
            {
               
                await _saleService.AddSaleWithStockAsyn(dto);
                return Ok(ApiResponse<SaleDto>.SuccessResponse(dto, "Sale and stock updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message, "Failed to process sale with stock"));
            }
        }


        [HttpPut]
        public IActionResult Update([FromBody] SaleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SaleDto>.FailResponse("Invalid sale data"));

            var success = _saleService.Update(dto);
            if (!success)
                return BadRequest(ApiResponse<SaleDto>.FailResponse("Failed to update sale"));

            return Ok(ApiResponse<SaleDto>.SuccessResponse(dto, "Sale updated successfully."));
        }



        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = _saleService.ChangeStatus(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse("Failed to change sale status"));

            return Ok(ApiResponse<string>.SuccessResponse("Status updated successfully."));
        }


     
    
    
    }

}
