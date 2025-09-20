using OnlineShop.Domain.Infrastructure;

namespace OnlineShop.Application.StockAdmin
{
	[Service]
	public class GetStock
    {
		private readonly IProductManager _productManager;

		public GetStock(IProductManager productManager)
		{
			_productManager = productManager;
		}

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; } 
            public IEnumerable<StockViewModel> Stock { get; set; } = Enumerable.Empty<StockViewModel>();
        }

		public IEnumerable<ProductViewModel> Do()
        {
            return _productManager.GetProductsWithStock(x =>
                new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Quantity = y.Quantity
                    })
                });
        }
    }
}
