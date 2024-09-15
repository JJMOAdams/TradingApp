namespace TradingApp.Entities
{
    public class Stock
    {
        public long Id { get; set; }
        public required string Symbol { get; set; }
        public DateTime BarDateTime { get; set; }
        public required string DayOfWeek { get; set; } // Monday, Tuesday, etc.
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public long TradeCount { get; set; }
        public decimal VWAP { get; set; }
    }
}