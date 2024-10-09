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

		public ProductViewModel Do(string name) =>
			_context.Products
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
							 InStock = y.Quantity >  0
						 })
			})
			.FirstOrDefault();

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
			public bool InStock { get; set; }
		}
    }
}
