using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ProductsAdmin;
using OnlineShop.Database;

namespace OnlineShop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Products
        [HttpGet("")]
        public IActionResult GetProducts() => Ok(new GetProducts(_context).Do());

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) => Ok(new GetProduct(_context).Do(id));

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Request request) => Ok(await new CreateProduct(_context).Do(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok(await new DeleteProduct(_context).Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct.Request request) => Ok(await new UpdateProduct(_context).Do(request));

    }
}
