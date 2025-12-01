using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using Inventory.BLL.Services;
using InventoryApi.Helper;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {

        private readonly IPurchaseServices _purchaseService;

        public PurchasesController(IPurchaseServices purchaseService)
        {
            _purchaseService = purchaseService;
        }

 
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _purchaseService.GetAll();
            return Ok(ApiResponse<IEnumerable<PurchaseDto>>.SuccessResponse(result, "Purchases fetched successfully."));
        }



        [HttpGet("Details")]
        public IActionResult GetAll(string invoiceNo)
        {
            var result = _purchaseService.GetPurchaseDetails(invoiceNo);
            return Ok(ApiResponse<IEnumerable<PurchaseDto>>.SuccessResponse(result, "Purchases fetched successfully."));
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _purchaseService.GetByID(id);
            if (result == null)
                return NotFound(ApiResponse<PurchaseDto>.FailResponse("Purchase not found"));

            return Ok(ApiResponse<PurchaseDto>.SuccessResponse(result, "Purchase fetched successfully."));
        }



        [HttpGet("ByInvoice/{invoiceNo}")]
        public IActionResult GetByInvoice(string invoiceNo)
        {
            var result = _purchaseService.GetByInvoice(invoiceNo);
            if (result == null)
                return NotFound(ApiResponse<PurchaseDto>.FailResponse("Purchase not found with given invoice"));

            return Ok(ApiResponse<PurchaseDto>.SuccessResponse(result, "Purchase fetched successfully."));
        }



        [HttpGet("BySupplier/{supplierId}")]
        public IActionResult GetPurchasesBySupplier(int supplierId)
        {
            var result = _purchaseService.GetPurchasesBySupplier(supplierId);
            return Ok(ApiResponse<IEnumerable<PurchaseDto>>.SuccessResponse(result, "Purchases by supplier fetched successfully."));
        }



        [HttpGet("ByDateRange")]
        public IActionResult GetPurchasesByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from == default || to == default)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid date range"));

            var result = _purchaseService.GetPurchasesByDateRange(from, to);
            return Ok(ApiResponse<IEnumerable<PurchaseDto>>.SuccessResponse(result, "Purchases in date range fetched successfully."));
        }


        [HttpPost]
        public async Task<IActionResult> AddPurchaseWithStock([FromBody] PurchaseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid purchase data"));

            try
            {
                await _purchaseService.AddPurchaseWithStock(dto);
                return Ok(ApiResponse<PurchaseDto>.SuccessResponse(dto, "Purchase and stock updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message, "Failed to process purchase with stock"));
            }
        }



        [HttpPut]
        public IActionResult Update([FromBody] PurchaseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<PurchaseDto>.FailResponse("Invalid purchase data"));

            var success = _purchaseService.Update(dto);
            if (!success)
                return BadRequest(ApiResponse<PurchaseDto>.FailResponse("Failed to update purchase"));

            return Ok(ApiResponse<PurchaseDto>.SuccessResponse(dto, "Purchase updated successfully."));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _purchaseService.ChangeStatus(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse("Failed to change purchase status"));

            return Ok(ApiResponse<string>.SuccessResponse("Status updated successfully."));
        }



        
    }


}
