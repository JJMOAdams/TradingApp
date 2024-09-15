namespace TradingApp.Helpers
{
    public class AlpacaMDResponse
    {
        public ICollection<BarData> bars { get; set; } = [];
        public string? next_page_token { get; set; }
        public required string currency { get; set; } 
    }

    public class BarData
    {
        public DateTime t { get; set; }
        public decimal o { get; set; }
        public decimal h { get; set; }
        public decimal l { get; set; }
        public decimal c { get; set; }
        public long v { get; set; }
        public long n { get; set; }
        public decimal vw { get; set; }
    }
}