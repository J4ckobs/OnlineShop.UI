using OnlineShop.Domain.Infrastructure;

namespace OnlineShop.Application.StockAdmin
{
	[Service]
	public class DeleteStock
    {
		private IStockManager _stockManager;

		public DeleteStock(IStockManager stockManager)
		{
			_stockManager = stockManager;
		}

		public Task<int> Do(int id)
        {
			return _stockManager.DeleteStock(id);
		}
    }
}
