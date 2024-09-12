using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ProductsAdmin;
using OnlineShop.Application.StockAdmin;
using OnlineShop.Database;
using System.Linq.Expressions;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace OnlineShop.UI.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        public AdminController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        
        //Products
        [HttpGet("products")]
        public IActionResult GetProducts() => Ok(new GetProducts(_context).Do());

        [HttpGet("products/{id}")]
        public IActionResult GetProduct(int id) => Ok(new GetProduct(_context).Do(id));

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Request request) => Ok(await new CreateProduct(_context).Do(request));

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok(await new DeleteProduct(_context).Do(id));

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct.Request request) => Ok(await new UpdateProduct(_context).Do(request));

        //Stock
        [HttpGet("stocks")]
        public IActionResult GetStock() => Ok(new GetStock(_context).Do());

        [HttpPost("stocks")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request request) => Ok(await new CreateStock(_context).Do(request));

	    [HttpDelete("stocks/{id}")]
		public async Task<IActionResult> DeleteStock(int id) => Ok(await new DeleteStock(_context).Do(id));

		[HttpPut("stocks")]
		public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request) => Ok(await new UpdateStock(_context).Do(request));
	}
}
