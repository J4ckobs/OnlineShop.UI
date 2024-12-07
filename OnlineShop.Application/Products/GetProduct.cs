using Microsoft.EntityFrameworkCore;
using OnlineShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Products
{
	public class GetProduct
	{
		private ApplicationDbContext _context;
		public GetProduct(ApplicationDbContext ctx)
		{
			_context = ctx;
		}

		public async Task<ProductViewModel> Do(string name)
		{
			var stocksOnHold = _context.StockOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

			if(stocksOnHold.Count > 0)
			{
				//var stockToReturn = _context.Stock.Where(x => stocksOnHold.Any(y => y.StockId == x.Id)).ToList();
				var stockIds = stocksOnHold.Select(y => y.StockId).ToList();
				var stockToReturn = _context.Stock.Where(x => stockIds.Contains(x.Id)).ToList();


				foreach (var stock in stockToReturn)
				{
					stock.Quantity = stock.Quantity + stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Quantity;
				}

				_context.StockOnHold.RemoveRange(stocksOnHold);

				await _context.SaveChangesAsync();
			}

			return _context.Products
				.Include(x=> x.Stock)
				.Where(x=> x.Name == name)
                .Select(x => new ProductViewModel
				{
					Name = x.Name,
					Description = x.Description,
					Value = $"{x.Value.ToString("N2")} $", // 1100.50 -> 1,100.50 -> 1,100.50 $

					Stock = x.Stock
						.Select(y=> new StockViewModel
						{
							Id = y.Id,
							Description = y.Description,
							Quantity = y.Quantity,
                        })
				})
				.FirstOrDefault() ?? new ProductViewModel { };
		}

		public class ProductViewModel
		{
			public string Name { get; set; }
			public string Description { get; set; } = string.Empty;
			public string Value { get; set; }

			public IEnumerable<StockViewModel> Stock { get; set; }
		}

        public class StockViewModel
		{
			public int Id { get; set; }
			public string Description { get; set; }
            public int Quantity { get; set; }
		}
    }
}
