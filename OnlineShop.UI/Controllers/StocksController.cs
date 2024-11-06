using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.StockAdmin;
using OnlineShop.Database;

namespace OnlineShop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class StocksController : Controller
    {
        private ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Stock
        [HttpGet("")]
        public IActionResult GetStock() => Ok(new GetStock(_context).Do());

        [HttpPost("")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request request) => Ok(await new CreateStock(_context).Do(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id) => Ok(await new DeleteStock(_context).Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request) => Ok(await new UpdateStock(_context).Do(request));
    }
}
