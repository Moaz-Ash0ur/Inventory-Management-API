using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface;
using InventoryApi.Helper;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            return Ok(ApiResponse<IEnumerable<CustomerDto>>.SuccessResponse(result, "Customers fetched successfully."));
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _customerService.GetByID(id);
            if (result == null)
                return NotFound(ApiResponse<CustomerDto>.FailResponse("Customer not found"));

            return Ok(ApiResponse<CustomerDto>.SuccessResponse(result, "Customer fetched successfully."));
        }


       
        [HttpGet("ByPhone/{phone}")]
        public IActionResult GetByPhone(string phone)
        {
            var result = _customerService.GetByPhone(phone);
            if (result == null)
                return NotFound(ApiResponse<CustomerDto>.FailResponse("Customer not found with given phone"));

            return Ok(ApiResponse<CustomerDto>.SuccessResponse(result, "Customer fetched successfully."));
        }


        [HttpGet("ByEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var result = _customerService.GetByEmail(email);
            if (result == null)
                return NotFound(ApiResponse<CustomerDto>.FailResponse("Customer not found with given email"));

            return Ok(ApiResponse<CustomerDto>.SuccessResponse(result, "Customer fetched successfully."));
        }



        [HttpGet("Top/{top}")]
        public IActionResult GetTopCustomers(int top)
        {
            var result = _customerService.GetTopCustomers(top);
            return Ok(ApiResponse<IEnumerable<CustomerDto>>.SuccessResponse(result, $"Top {top} customers fetched successfully."));
        }


        [HttpPost]
        public IActionResult Create([FromBody] CustomerDto dto)
        {
             var exsitEmail = GetByEmail(dto.Email);
             if(exsitEmail != null)
                return BadRequest(ApiResponse<CustomerDto>.FailResponse("Try another email"));


            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<CustomerDto>.FailResponse("Invalid customer data"));

            var success = _customerService.Insert(dto, out int id);
            if (!success)
                return BadRequest(ApiResponse<CustomerDto>.FailResponse("Failed to create customer"));

            dto.Id = id;
            return Ok(ApiResponse<CustomerDto>.SuccessResponse(dto, "Customer created successfully."));
        }


        [HttpPut]
        public IActionResult Update([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<CustomerDto>.FailResponse("Invalid customer data"));

            var success = _customerService.Update(dto);
            if (!success)
                return BadRequest(ApiResponse<CustomerDto>.FailResponse("Failed to update customer"));

            return Ok(ApiResponse<CustomerDto>.SuccessResponse(dto, "Customer updated successfully."));
        }



        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = _customerService.ChangeStatus(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse("Failed to change customer status"));

            return Ok(ApiResponse<string>.SuccessResponse("Status updated successfully."));
        }
    }



}
