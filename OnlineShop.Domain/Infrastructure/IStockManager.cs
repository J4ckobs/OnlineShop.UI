using OnlineShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Infrastructure
{
	public interface IStockManager
	{
		bool EnoughStock(int stockId, int qty);

		Stock GetStockWithProduct(int stockId);

		Task<int> CreateStock(Stock stock);
		Task<int> DeleteStock(int id);
		Task<int> UpdateStockRange(List<Stock> stockList); 

		Task PutStockOnHold(int stockId, int qty, string sessionId);
		Task RemoveStockFromHold(string sessionId);
		Task RemoveStockFromHold(int stockId, int qty, string sessionId);
		Task RetriveExpiredStockOnHold();
	}
}