using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Entities
{
    public class StockWatch
    {
        public int Id { get; set; }
        public required string Symbol { get; set; }
        public bool IsWatched { get; set; } = true;
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Score { get; set; } // custom scoring algorithm
    }
}