using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradingApp.Attributes;
using TradingApp.Contexts;
using TradingApp.Entities;
using TradingApp.Helpers;

namespace TradingApp.Controllers
{
    [ActiveUser]
    public class StockController(DataContext context, AlpacaMDContext alpacaMDContext) : BaseController
    {
        private readonly DataContext _context = context;
        private readonly AlpacaMDContext _alpacaMDContext = alpacaMDContext;

        [HttpGet("stocks")]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _context.StockWatches.ToListAsync();
            return Ok(stocks);
        }

        [HttpGet("stocks/{symbol}")]
        public async Task<IActionResult> GetStock(string symbol)
        {
            var stock = await _context.Stocks.Where(s => s.Symbol == symbol).Take(180).ToListAsync();
            return Ok(stock);
        }

        [HttpPost("stocks/{symbol}")]
        public async Task<IActionResult> AddStock(string symbol)
        {
            var stock = await _context.StockWatches.FirstOrDefaultAsync(s => s.Symbol == symbol);
            if (stock != null) return BadRequest("Stock already exists");

            AlpacaMDRequest request = new()
            {
                Symbol = symbol,
                TimeFrame = "1D",
                Limit = 180,
                AsOf = DateTime.Today - TimeSpan.FromDays(180)
            };

            var response = await _alpacaMDContext.GetBars(request);
            if (response == null) return BadRequest("Failed to get stock data");

            var stocks = response.bars.Select(b => new Stock
            {
                Symbol = symbol,
                BarDateTime = b.t,
                Open = b.o,
                High = b.h,
                Low = b.l,
                Close = b.c,
                Volume = b.v,
                DayOfWeek = b.t.DayOfWeek.ToString(),
                VWAP = b.vw
            });

            await _context.Stocks.AddRangeAsync(stocks);
            await _context.SaveChangesAsync();

            var stockWatch = new StockWatch
            {
                Symbol = symbol,
                LastUpdated = DateTime.Now,
                Price = stocks.Last().Close,
                Score = 0, // Placeholder TODO: Implement scoring
                IsWatched = true
            };

            return Ok();
        }
    }
}