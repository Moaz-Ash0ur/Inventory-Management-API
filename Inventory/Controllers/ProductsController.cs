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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result, "Products fetched successfully."));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetByID(id);
            if (result == null)
                return NotFound(ApiResponse<ProductDto>.FailResponse("Product not found"));

            return Ok(ApiResponse<ProductDto>.SuccessResponse(result, "Product fetched successfully."));
        }

        [HttpGet("BySKU/{sku}")]
        public IActionResult GetBySKU(string sku)
        {
            var result = _productService.GetBySKU(sku);
            if (result == null)
                return NotFound(ApiResponse<ProductDto>.FailResponse("Product not found with given SKU"));

            return Ok(ApiResponse<ProductDto>.SuccessResponse(result, "Product fetched successfully."));
        }

        //[HttpPost]
        //public IActionResult Add([FromBody] ProductDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ApiResponse<ProductDto>.FailResponse("Invalid product data"));

        //    var success = _productService.Insert(dto, out int id);
        //    if (!success)
        //        return BadRequest(ApiResponse<ProductDto>.FailResponse("Failed to create product"));

        //    dto.Id = id; 
        //    return Ok(ApiResponse<ProductDto>.SuccessResponse(dto, "Product created successfully."));
        //}


        [HttpPost]
        public IActionResult Add([FromForm] ProductDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<ProductDto>.FailResponse("Invalid product data."));

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageName = UploadFile.UploadImage(dto.Image);
                dto.ImageUrl = imageName;
            }

            if (_productService.Insert(dto, out int id))
            {
                dto.Id = id;

                if (!string.IsNullOrEmpty(dto.ImageUrl))
                    dto.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/Products/{dto.ImageUrl}";

                return CreatedAtAction(nameof(GetById), new { id },
                    ApiResponse<ProductDto>.SuccessResponse(dto, "Product created successfully."));
            }

            return BadRequest(ApiResponse<ProductDto>.FailResponse("Failed to add product."));
        }



        //[HttpPut]
        //public IActionResult Update([FromBody] ProductDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ApiResponse<ProductDto>.FailResponse("Invalid product data"));

        //    var success = _productService.Update(dto);
        //    if (!success)
        //        return BadRequest(ApiResponse<ProductDto>.FailResponse("Failed to update product"));

        //    return Ok(ApiResponse<ProductDto>.SuccessResponse(dto, "Product updated successfully."));
        //}


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] ProductDto dto)
        {
            var existingProduct = _productService.GetByID(id);
            if (existingProduct == null)
                return NotFound(ApiResponse<string>.FailResponse("Product not found."));

            if (dto.Image != null && dto.Image.Length > 0)
            {
                dto.ImageUrl = UploadFile.UploadImage(dto.Image, existingProduct.ImageUrl);
            }
            else
            {
                dto.ImageUrl = existingProduct.ImageUrl;
            }
            dto.Id = id;
            if (_productService.Update(dto))
                return Ok(ApiResponse<ProductDto>.SuccessResponse(dto, "Product updated successfully."));

            return BadRequest(ApiResponse<string>.FailResponse("Failed to update product."));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _productService.ChangeStatus(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse("Failed to change product status"));

            return Ok(ApiResponse<string>.SuccessResponse("Status updated successfully."));
        }


        [HttpGet("MostSold")]
        public IActionResult GetMostSoldProducts([FromQuery] int top = 5)
        {
            var result = _productService.GetMostSoldProducts(top);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result, "Most sold products fetched successfully."));
        }

        [HttpGet("LowStock")]
        public IActionResult GetLowStockProducts()
        {
            var result = _productService.GetLowStockProducts();
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result, "Low stock products fetched successfully."));
        }


    }



}
