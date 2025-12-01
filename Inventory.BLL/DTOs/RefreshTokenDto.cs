namespace Inventory.BLL.DTOs
{
    public class RefreshTokenDto : BaseDto
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime ExpiresAt { get; set; }
        public int CurrentState { get; set; }


    }






}
