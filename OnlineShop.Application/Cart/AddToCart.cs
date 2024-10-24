using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineShop.Database;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.Cart
{
	public class AddToCart
	{
        private ISession _session;
        private ApplicationDbContext _context;

        public AddToCart(ISession session, ApplicationDbContext context)
        {
            _session = session;
            _context = context;
        }
        public class Request
        {
            public int StockId { get; set; }
            public int Quantity { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
			var stockOnHold = _context.StockOnHold.Where(x => x.SessionId == _session.Id).ToList();
			var stockToHold = _context.Stock.Where(x => x.Id == request.StockId).FirstOrDefault();

            if(stockToHold.Quantity < request.Quantity)
                return false;


            _context.StockOnHold.Add(new StockOnHold
            {
                StockId = stockToHold.Id,
                SessionId = _session.Id,
                Quantity = request.Quantity,
                ExpiryDate = DateTime.Now.AddMinutes(20)
            });

            stockToHold.Quantity = stockToHold.Quantity - request.Quantity;

            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            await _context.SaveChangesAsync();


		    var cartList = new List<CartProduct>();
			var stringObject = _session.GetString("cart");

			if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
			}

            if(cartList.Any(x => x.StockId == request.StockId))
            {
                cartList.Find(x => x.StockId == request.StockId).Quantity += request.Quantity;
            }
            else
            {
                cartList.Add(new CartProduct
                {
                    StockId = request.StockId,
                    Quantity = request.Quantity 
                });
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);

            return true;
        }
    }
}
