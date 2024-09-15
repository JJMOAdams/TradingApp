namespace TradingApp.Helpers
{
    public class AlpacaMDRequest
    {
        public required string Symbol { get; set; }
        public required string TimeFrame { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Limit { get; set; }
        public string? Adjustment { get; set; }
        public DateTime? AsOf { get; set; }
        public string? Feed { get; set; }
        public string? Format { get; set; }
        public string? PageToken { get; set; }
        public string? Sort { get; set; } = "asc";
    }
}