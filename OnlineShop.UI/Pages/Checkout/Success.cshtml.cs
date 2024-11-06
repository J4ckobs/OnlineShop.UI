using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;
using OnlineShop.Database;

namespace OnlineShop.UI.Pages.Checkout
{
    public class SuccessModel : PageModel
    {
		private ApplicationDbContext _context;
		public SuccessModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			new ClearCart(HttpContext.Session).Do();

			return Page();
		}
    }
}
