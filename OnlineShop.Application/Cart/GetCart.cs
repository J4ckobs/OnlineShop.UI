using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShop.Database;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.Cart
{
	public class GetCart
	{
		private ISession _session;
		private ApplicationDbContext _context;
		public GetCart(ISession session, ApplicationDbContext context)
		{
			_session = session;
			_context = context;
		}
		public class Response
		{
            public string Name { get; set; }
            public string Value { get; set; }
			public decimal RealValue { get; set; }
			public int StockId { get; set; }
			public int Quantity { get; set; }
		}

		public IEnumerable<Response> Do()
		{
			var stringObject = _session.GetString("cart");

			if (string.IsNullOrEmpty(stringObject))
				return new List<Response>();

			var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

			// Server Site Request
			var stockIds = cartList.Select(y => y.StockId).ToList();

			var stocks = _context.Stock
				.Include(x => x.Product)
				.Where(x => stockIds.Contains(x.Id))
				.ToList();

			// Client Side Operations
			var response = stocks
				.Select(x => new Response
				{
					Name = x.Product.Name,
					Value = $"{x.Product.Value.ToString("N2")} $",
					RealValue = x.Product.Value,
					StockId = x.Id,
					Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id)?.Quantity ?? 0  // Dopasowanie ilości po stronie klienta
				})
				.ToList();

			return response;
		}
	}
}
