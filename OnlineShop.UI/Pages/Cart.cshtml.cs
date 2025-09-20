using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;

namespace OnlineShop.UI.Pages
{
    public class CartModel : PageModel
    {

        public IEnumerable<GetCart.Response> Cart { get; set; }
        
        public IActionResult OnGet(
            [FromServices] GetCart getCart)
        {
            Cart = getCart.Do();

            return Page();
        }
    }
}
