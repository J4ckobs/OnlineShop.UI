using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Cart;
using OnlineShop.Database;

namespace OnlineShop.UI.ViewComponents
{
	public class CartViewComponent : ViewComponent
	{
        private ApplicationDbContext _context;
        public CartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string view = "Default")
        {
            return View(view, new GetCart(HttpContext.Session, _context).Do());
        }
    }
}
