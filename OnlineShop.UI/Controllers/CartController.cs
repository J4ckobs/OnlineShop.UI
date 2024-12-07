using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Cart;
using OnlineShop.Database;

namespace OnlineShop.UI.Controllers
{
	[Route("[controller]/[action]")]
	public class CartController : Controller
	{
		private ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{stockId}")]
		public async Task<IActionResult> AddOne(int stockId)
		{
			var request = new AddToCart.Request
			{
				StockId = stockId,
				Quantity = 1
			};

			var addToCart = new AddToCart(HttpContext.Session, _context);

			var success = await addToCart.DoAsync(request);

			if (success)
				return Ok("Item added to cart");

			return BadRequest("Failed to add item to cart");
		}

		[HttpPost("{stockId}")]
		public async Task<IActionResult> RemoveOne(int stockId)
		{
			var request = new RemoveFromCart.Request
			{
				StockId = stockId,
				Quantity = 1
			};

			var addToCart = new RemoveFromCart(HttpContext.Session, _context);

			var success = await addToCart.DoAsync(request);

			if (success)
				return Ok("Item removed from cart");

			return BadRequest("Failed to remove item from cart");
		}

		[HttpPost("{stockId}")]
		public async Task<IActionResult> RemoveAll(int stockId)
		{
			var request = new RemoveFromCart.Request
			{
				StockId = stockId,
				RemoveAll = true
			};

			var addToCart = new RemoveFromCart(HttpContext.Session, _context);

			var success = await addToCart.DoAsync(request);

			if (success)
				return Ok("All items removed from cart");

			return BadRequest("Failed to remove all items from cart");
		}
	}
}
