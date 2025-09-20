using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;

namespace OnlineShop.Application.StockAdmin
{
	[Service]
	public class CreateStock
    {
        private IStockManager _stockManager;

        public CreateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock
            {
				ProductId = request.ProductId,
				Description = request.Description,
                Quantity = request.Quantity
            };
            
            await _stockManager.CreateStock(stock);

            return new Response
            {
                Id = stock.Id,
                Description = request.Description,
                Quantity = request.Quantity
            };
        }

        public class Request
        {
            public int ProductId { get; set; }
			public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
		}

        public class Response
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }
    }
}
