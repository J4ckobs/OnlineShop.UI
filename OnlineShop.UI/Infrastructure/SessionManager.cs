using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;
using static OnlineShop.Application.Orders.CreateOrder;


namespace OnlineShop.UI.Infrastructure
{
	public class SessionManager : ISessionManager
	{
		private ISession _session;

		public SessionManager(IHttpContextAccessor httpContextAccessor)
		{
			_session = httpContextAccessor.HttpContext.Session;
		}

		public string GetId() => _session.Id;

		public void AddProduct(CartProduct cartProduct)
		{
			var cartList = new List<CartProduct>();
			var stringObject = _session.GetString("cart");

			if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
			}

            if(cartList.Any(x => x.StockId == cartProduct.StockId))
            {
                cartList.Find(x => x.StockId == cartProduct.StockId).Quantity += cartProduct.Quantity;
            }
            else
            {
                cartList.Add(new CartProduct
                {
					ProductId = cartProduct.ProductId,
					ProductName = cartProduct.ProductName,
                    StockId = cartProduct.StockId,
                    Quantity = cartProduct.Quantity,
					Value = cartProduct.Value
				});
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
		}


		public IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector)
		{
			var stringObject = _session.GetString("cart");

			if (string.IsNullOrEmpty(stringObject))
				return new List<TResult>();

			var cartList = JsonConvert.DeserializeObject<IEnumerable<CartProduct>>(stringObject);

			return cartList.Select(selector);
		}

		public void AddCustomerInformation(CustomerInformation customer)
		{
			var stringObject = JsonConvert.SerializeObject(customer);

			_session.SetString("customer-info", stringObject);
		}

		public CustomerInformation GetCustomerInformation(/*ISession session*/)
		{
			var stringObject = _session.GetString("customer-info");

			if (string.IsNullOrEmpty(stringObject))
				return null;

			var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);

			return customerInformation;

		}

		public void RemoveProduct(int stockId, int qty)
		{
			var cartList = new List<CartProduct>();
			var stringObject = _session.GetString("cart");

			if (string.IsNullOrEmpty(stringObject)) return;

			cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

			if (!cartList.Any(x => x.StockId == stockId))
				return;

			var product = cartList.First(x => x.StockId == stockId);

			product.Quantity -= qty;

			if(product.Quantity <= 0)
				cartList.Remove(product);

			//Cart update | session site
			stringObject = JsonConvert.SerializeObject(cartList);

			_session.SetString("cart", stringObject);

			return;
		}

		public void ClearCart()
		{
			_session.Remove("cart");
		}
	}
}
