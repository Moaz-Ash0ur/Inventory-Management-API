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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _supplierService.GetAll();
            return Ok(ApiResponse<IEnumerable<SupplierDto>>.SuccessResponse(result, "Suppliers fetched successfully."));
        }



        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _supplierService.GetByID(id);
            if (result == null)
                return NotFound(ApiResponse<SupplierDto>.FailResponse("Supplier not found"));

            return Ok(ApiResponse<SupplierDto>.SuccessResponse(result, "Supplier fetched successfully."));
        }



        [HttpGet("ByPhone/{phone}")]
        public IActionResult GetByPhone(string phone)
        {
            var result = _supplierService.GetByPhone(phone);
            if (result == null)
                return NotFound(ApiResponse<SupplierDto>.FailResponse("Supplier not found with given phone"));

            return Ok(ApiResponse<SupplierDto>.SuccessResponse(result, "Supplier fetched successfully."));
        }



        [HttpGet("ByEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var result = _supplierService.GetByEmail(email);
            if (result == null)
                return NotFound(ApiResponse<SupplierDto>.FailResponse("Supplier not found with given email"));

            return Ok(ApiResponse<SupplierDto>.SuccessResponse(result, "Supplier fetched successfully."));
        }


        [HttpPost]
        public IActionResult Create([FromBody] SupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SupplierDto>.FailResponse("Invalid supplier data"));

            var success = _supplierService.Insert(dto, out int id);
            if (!success)
                return BadRequest(ApiResponse<SupplierDto>.FailResponse("Failed to create supplier"));

            dto.Id = id;
            return Ok(ApiResponse<SupplierDto>.SuccessResponse(dto, "Supplier created successfully."));
        }


        [HttpPut]
        public IActionResult Update([FromBody] SupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SupplierDto>.FailResponse("Invalid supplier data"));

            var success = _supplierService.Update(dto);
            if (!success)
                return BadRequest(ApiResponse<SupplierDto>.FailResponse("Failed to update supplier"));

            return Ok(ApiResponse<SupplierDto>.SuccessResponse(dto, "Supplier updated successfully."));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _supplierService.ChangeStatus(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse("Failed to change supplier status"));

            return Ok(ApiResponse<string>.SuccessResponse("Status updated successfully."));
        }
    
    }



}
