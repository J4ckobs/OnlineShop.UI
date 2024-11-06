using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;
using OnlineShop.Application.Products;
using OnlineShop.Database;

namespace OnlineShop.UI.Pages
{
    public class ProductModel : PageModel
    {
        private ApplicationDbContext _context;

        public ProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }

        public GetProduct.ProductViewModel Product { get; set; }

        public async Task<IActionResult> OnGet(string name)
        {
            Product = await new GetProduct(_context).Do(name.Replace("-"," "));

            if (Product == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("###");
			Console.WriteLine(CartViewModel.Quantity);
			Console.WriteLine("###");

			var stockAdded = await new AddToCart(HttpContext.Session, _context).DoAsync(CartViewModel);

            if(stockAdded)
                return RedirectToPage("Cart");
            else
                //ToDo: Add warning
                return Page();
        }
    }
}
