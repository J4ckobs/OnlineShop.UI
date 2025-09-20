using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;
using System.Linq.Expressions;

namespace OnlineShop.Application.OrderAdmin
{
	[Service]
	public class GetOrder
	{
		private IOrderManager _ordderManager;

		public GetOrder(IOrderManager ordderManager)
		{
			_ordderManager = ordderManager;
		}

		public class Response
		{
			public int Id { get; set; }
			public string OrderRef { get; set; }
			public string Stripereference { get; set; }

			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }

			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string PostCode { get; set; }

			public IEnumerable<Product> Products { get; set; }
		}

		public class Product
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public int Quantity { get; set; }
			public decimal Price { get; set; }
			public string StockDescription { get; set; }
		}

		public Response Do(int id) =>
			_ordderManager.GetOrderById(id, Projection);

		private static Expression<Func<Order, Response>> Projection = (order) =>
			new Response
			{
				Id = order.Id,
				OrderRef = order.OrderRef,
				Stripereference = order.StripeReference,

				FirstName = order.FirstName,
				LastName = order.LastName,
				Email = order.Email,
				PhoneNumber = order.PhoneNumber,

				Address1 = order.Address1,
				Address2 = order.Address2,
				City = order.City,
				PostCode = order.PostCode,

				Products = order.OrderStocks.Select(prod => new Product
				{
					Name = prod.Stock.Product.Name,
					Description = prod.Stock.Product.Description,
					Quantity = prod.Quantity,
					Price = prod.Stock.Product.Value,
					StockDescription = prod.Stock.Description
				})
			};
	}
}

