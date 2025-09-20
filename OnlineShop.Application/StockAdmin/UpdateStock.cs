using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.StockAdmin
{
	[Service]
	public class UpdateStock
    {
		private IStockManager _stockManager;

		public UpdateStock(IStockManager stockManager)
		{
			_stockManager = stockManager;
		}

		public async Task<Response> Do(Request request)
        {
            var stockList = new List<Stock>();

            foreach (var stock in request.Stock)
            {
                stockList.Add(new Stock
                {
                    Id = stock.Id,
                    Description = stock.Description,
                    Quantity = stock.Quantity,
                    ProductId = stock.ProductId,
                });
            }

            await _stockManager.UpdateStockRange(stockList);

            return new Response
            {
                Stock = request.Stock,
            };
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public int ProductId { get; set; }
        }

        public class Request
        {
            public IEnumerable<StockViewModel> Stock { get; set; } = Enumerable.Empty<StockViewModel>();
        }

        public class Response
        {
            public IEnumerable<StockViewModel> Stock { get; set; } = Enumerable.Empty<StockViewModel>();
        }
    }
}
