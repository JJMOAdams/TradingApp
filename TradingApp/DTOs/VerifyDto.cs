namespace TradingApp.DTOs
{
    public class VerifyDto
    {
        public required string Username { get; set; }
        public required string ChallengeCode { get; set; }
    }
}