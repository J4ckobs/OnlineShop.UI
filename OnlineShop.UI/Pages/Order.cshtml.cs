using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Database;
using OnlineShop.Application.Orders;

namespace OnlineShop.UI.Pages
{
    public class OrderModel : PageModel
    {
		private ApplicationDbContext _context;

		public OrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public GetOrder.Response Order {  get; set; }

        public void OnGet(string reference)
        {
            Order = new GetOrder(_context).Do(reference);
        }
    }
}
