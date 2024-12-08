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

			var removeFromCart = new RemoveFromCart(HttpContext.Session, _context);

			var success = await removeFromCart.DoAsync(request);

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

			var removeAllFromCart = new RemoveFromCart(HttpContext.Session, _context);

			var success = await removeAllFromCart.DoAsync(request);

			if (success)
				return Ok("Cart cleared"); // TO DO: return value decreased by value of removed items when more items in cart

			return BadRequest("Failed to cleared cart");
		}
	}
}
