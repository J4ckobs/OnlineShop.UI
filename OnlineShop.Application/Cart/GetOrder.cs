using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShop.Database;
using OnlineShop.Domain.Models;
using System.Text;

namespace OnlineShop.Application.Cart
{
	public class GetOrder
	{
		private ISession _session;
		private ApplicationDbContext _context;
		public GetOrder(ISession session, ApplicationDbContext context)
		{
			_session = session;
			_context = context;
		}

		public class Response
		{
			public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }

			public int GetTotalCharge() => Products.Sum(x => x.Value * x.Quantity);
        }

		public class Product
		{
			public int ProductId { get; set; }
			public int StockId { get; set; }
			public int Quantity { get; set; }
            public int Value { get; set; }
        }

		public class CustomerInformation
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }

			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string PostCode { get; set; }
		}

		public Response Do()
		{
			var cart = _session.GetString("cart");

			if (cart == null)
				return new Response { };

			var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(cart);

			var stockIds = cartList.Select(x => x.StockId).ToList();

			var stock = _context.Stock
				.Include(x => x.Product)
				.Where(x => stockIds.Contains(x.Id))
				.ToList();

			var listOfProducts = stock
				.Select(x => new Product
				{
					ProductId = x.ProductId,
					StockId = x.Id,
					Value = Convert.ToInt32(x.Product.Value * 100),
					Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id).Quantity

				}).ToList();


			var customerInfoString = _session.GetString("customer-info");

			var customerInformation = JsonConvert.DeserializeObject<OnlineShop.Domain.Models.CustomerInformation>(customerInfoString);

			return new Response
			{ 
				Products = listOfProducts,
				CustomerInformation = new CustomerInformation
				{
					FirstName = customerInformation.FirstName,
					LastName = customerInformation.LastName,
					Email = customerInformation.Email,
					PhoneNumber = customerInformation.PhoneNumber,

					Address1 = customerInformation.Address1,
					Address2 = customerInformation.Address2,
					City = customerInformation.City,
					PostCode = customerInformation.PostCode,

				}
			};
		}
	}
}