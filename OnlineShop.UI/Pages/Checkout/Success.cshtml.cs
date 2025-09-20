using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;

namespace OnlineShop.UI.Pages.Checkout
{
    public class SuccessModel : PageModel
    {
		public IActionResult OnGet(
			[FromServices] ClearCart clearCart)
		{
			clearCart.Do();

			return Page();
		}
    }
}
