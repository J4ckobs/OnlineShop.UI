using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.Products
{
	[Service]
	public class GetProduct
	{
		private IStockManager _stockManager;
		private IProductManager _productManager;

		public GetProduct(IStockManager stockManager, IProductManager productManager)
		{
			_stockManager = stockManager;
			_productManager = productManager;
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

		public async Task<ProductViewModel> Do(string name)
		{
			await _stockManager.RetriveExpiredStockOnHold();

			return _productManager.GetProductByName(name, Projection);
		}

		private Func<Product, ProductViewModel> Projection = (product) => new ProductViewModel
		{
			Name = product.Name,
			Description = product.Description,
			Value = $"{product.Value.ToString("N2")} $", // 1100.50 -> 1,100.50 -> 1,100.50 $

			Stock = product.Stock
					.Select(y => new StockViewModel
					{
						Id = y.Id,
						Description = y.Description,
						Quantity = y.Quantity,
					})
		};
	}
}
