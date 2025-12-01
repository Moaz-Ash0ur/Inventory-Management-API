namespace Inventory.BLL.DTOs
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
