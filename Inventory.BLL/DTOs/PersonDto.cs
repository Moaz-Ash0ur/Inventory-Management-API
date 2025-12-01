namespace Inventory.BLL.DTOs
{
    public class PersonDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

}
