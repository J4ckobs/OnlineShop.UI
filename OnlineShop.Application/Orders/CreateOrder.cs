using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using OnlineShop.Database;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Orders
{
	public class CreateOrder
	{
		private ApplicationDbContext _context;

		public CreateOrder(ApplicationDbContext context)
        {
            _context = context;
        }

		public class Request
		{
            public string StripeReference { get; set; }
            public string SessionId { get; set; }

            public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }

			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string PostCode { get; set; }

			public List<Stock> Stocks { get; set; }
		}

		public class Stock
		{
            public int StockId { get; set; }
			public int Quantity { get; set; }
        }

		public async Task<bool> Do(Request request)
		{
            //var stockIds = request.Stocks.Select(x => x.StockId).ToList();

            var stockOnHold = _context.StockOnHold
                .Where(x => x.SessionId == request.SessionId)
                .ToList();

			_context.StockOnHold.RemoveRange(stockOnHold);

            var order = new Order
			{
				OrderRef = CreateOrderReferance(),
				StripeReference = request.StripeReference,

				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber,

				Address1 = request.Address1,
				Address2 = request.Address2,
				City = request.City,
				PostCode = request.PostCode,

				OrderStocks = request.Stocks.Select(x => new OrderStock
				{
					StockId = x.StockId,
					Quantity = x.Quantity,
					
				}).ToList()
			};

			_context.Orders.Add(order);

			return await _context.SaveChangesAsync() > 0;
		}

		public string CreateOrderReferance()
		{
			var chars = "ABCDEFGHIJKLMNOPRSTUVWXYZabscefghijklmnoprstuvwxyz0123456789";
			var result = new char[12];
			var random = new Random();

			do
			{
				for (int i = 0; i < result.Length; i++)
					result[i] = chars[random.Next(chars.Length)];
			}
			while (_context.Orders.Any(x => x.OrderRef == new string(result)));
			
			return new string(result);
		}
    }
}
