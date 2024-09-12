using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.ProductsAdmin;
using OnlineShop.Database;

namespace OnlineShop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        [BindProperty] //Znacznik g³ównego modelu
        public CreateProduct.Request Product { get; set; } = new CreateProduct.Request();

        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }

        public void OnGet()
        {
            Products = new GetProducts(_context).Do();
        }

        public async Task<IActionResult> OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
