namespace Inventory.BLL.DTOs.User
{
    public class UserResultDto
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }


}
