using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineShop.Database;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.Cart
{
	public class RemoveFromCart
	{
        private ISession _session;
        private ApplicationDbContext _context;

        public RemoveFromCart(ISession session, ApplicationDbContext context)
        {
            _session = session;
            _context = context;
        }
        public class Request
        {
            public int StockId { get; set; }
            public int Quantity { get; set; }
            public bool RemoveAll { get; set; }
        }

        public async Task<bool> DoAsync(Request request)
        {
			var cartList = new List<CartProduct>();
			var stringObject = _session.GetString("cart");

			if (string.IsNullOrEmpty(stringObject))
                return true;
            
            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if(!cartList.Any(x => x.StockId == request.StockId))
                return true;

            var stockOnHold = _context.StockOnHold
                .FirstOrDefault(x => x.StockId == request.StockId
                                && x.SessionId == _session.Id);

            var stock = _context.Stock.FirstOrDefault(x => x.Id == request.StockId);

			if (request.RemoveAll || stockOnHold.Quantity <= 1)
            {
                cartList.RemoveAll(x => x.StockId == request.StockId);

                stock.Quantity += stockOnHold.Quantity;
                stockOnHold.Quantity = 0;
            }
            else// if (stockOnHold.Quantity == 1 && request.Quantity >=1)
            {
				cartList.Find(x => x.StockId == request.StockId).Quantity -= request.Quantity;

				stock.Quantity += request.Quantity;
                stockOnHold.Quantity -= request.Quantity;
            }

            //Cart update | session site
			stringObject = JsonConvert.SerializeObject(cartList);

			_session.SetString("cart", stringObject);

			if (stockOnHold.Quantity <= 0)
				_context.StockOnHold.Remove(stockOnHold);

			await _context.SaveChangesAsync();

            return true;
        }

        void RemoveItem()
        {

        }
    }
}
