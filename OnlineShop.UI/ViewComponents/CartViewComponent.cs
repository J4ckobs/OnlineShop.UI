using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Cart;

namespace OnlineShop.UI.ViewComponents
{
	public class CartViewComponent : ViewComponent
	{
		private GetCart _getCart;

		public CartViewComponent(GetCart getCart)
        {
            _getCart = getCart;
        }
        public IViewComponentResult Invoke(string view = "Default")
        {
            if(view == "Small")
            {
                var totalValue = _getCart.Do().Sum(x => x.RealValue * x.Quantity);

				return View(view, $"{totalValue} $");
            }

            return View(view, _getCart.Do());
        }
    }
}
