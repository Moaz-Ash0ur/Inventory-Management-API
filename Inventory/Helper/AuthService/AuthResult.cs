namespace InventoryApi.Helper
{

    public class AuthResult
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }







}
