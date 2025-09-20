using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.Products
{
	[Service]
	public class GetProducts
    {
		private IProductManager _productManager;

		public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }
       
        public class ProductViewModel
        {
			public string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public string Value { get; set; }
            public int StockCount { get; set; }
        }

        public IEnumerable<ProductViewModel> Do() =>
            _productManager.GetProductsWithStock(Projection);

        private Func<Product, ProductViewModel> Projection = (product) => new ProductViewModel
        {
            Name = product.Name,
            Description = product.Description,
            Value = product.Value.GetValueString(),
            StockCount = product.Stock.Sum(y => y.Quantity)
        };

	}
}
