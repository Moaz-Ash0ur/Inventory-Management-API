using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.Interface;
using InventoryApi.Helper;
using Inventory.BLL.DTOs;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       private readonly ICategories _categoryService;

        public CategoriesController(ICategories categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = _categoryService.GetAll();
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories, "Categories retrieved successfully."));
        }

        /// <summary>
        /// Get category by Id.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CategoryDto>> GetById(int id)
        {
            var category = _categoryService.GetByID(id);
            if (category == null)
                return NotFound(ApiResponse<CategoryDto>.FailResponse($"Category with id={id} not found.", "Category not found."));

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(category, "Category retrieved successfully."));
        }

        /// <summary>
        /// Create new category.
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<CategoryDto>> Create(CategoryDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<CategoryDto>.FailResponse("Invalid category data."));

            if (_categoryService.Insert(dto, out int id))
            {
                dto.Id = id;
                return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<CategoryDto>.SuccessResponse(dto, "Category created successfully."));
            }

            return BadRequest(ApiResponse<CategoryDto>.FailResponse("Failed to Add category."));
        }

        /// <summary>
        /// Update existing category.
        /// </summary>
        [HttpPut("{categoryId}")]
        public ActionResult<ApiResponse<CategoryDto>> Update(int categoryId, CategoryDto dto)
        {
            var existing = _categoryService.GetByID(categoryId);
            if (existing == null)
                return NotFound(ApiResponse<CategoryDto>.FailResponse($"Category with id={categoryId} not found.", "Category not found."));

            dto.Id = categoryId;
            var updated = _categoryService.Update(dto);
            if (!updated)
                return BadRequest(ApiResponse<CategoryDto>.FailResponse("Failed to update category.", "Update error."));

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(dto, "Category updated successfully."));
        }

        /// <summary>
        /// Delete category by Id.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object>> Delete(int id)
        {
            var existing = _categoryService.GetByID(id);
            if (existing == null)
                return NotFound(ApiResponse<object>.FailResponse($"Category with id={id} not found.", "Category not found."));

            var deleted = _categoryService.ChangeStatus(id);
            if (!deleted)
                return BadRequest(ApiResponse<object>.FailResponse("Failed to delete category.", "Delete error."));

            return Ok(ApiResponse<object>.SuccessResponse("Category deleted successfully."));
        }

    }
}
