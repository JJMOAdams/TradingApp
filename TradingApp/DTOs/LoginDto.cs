namespace TradingApp.DTOs
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public required string ToTPCode { get; set; }
    }
}