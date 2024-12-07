using Microsoft.AspNetCore.Http.HttpResults;
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
            if(view == "Small")
            {
                var totalValue = new GetCart(HttpContext.Session, _context).Do().Sum(x => x.RealValue * x.Quantity);

				return View(view, $"{totalValue} $");
            }

            return View(view, new GetCart(HttpContext.Session, _context).Do());
        }
    }
}
