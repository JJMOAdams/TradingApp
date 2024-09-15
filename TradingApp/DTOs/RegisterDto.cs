namespace TradingApp.DTOs
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string RegistrationCode { get; set; }
    }
}