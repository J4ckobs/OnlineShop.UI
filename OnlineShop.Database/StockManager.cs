using OnlineShop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OnlineShop.Domain.Infrastructure;

namespace OnlineShop.Database
{
	public class StockManager : IStockManager
    {
        public ApplicationDbContext _context;

        public StockManager(ApplicationDbContext context)
        {
            _context = context;
        }

		public bool EnoughStock(int stockId, int qty)
		{
			return _context.Stock.FirstOrDefault(x => x.Id == stockId).Quantity >= qty;
		}

		public Task<int> CreateStock(Stock stock)
		{
			_context.Stock.Add(stock);

			return _context.SaveChangesAsync();
		}

		public Task<int> DeleteStock(int id)
		{
			var stock = _context.Stock.FirstOrDefault(x => x.Id == id);

			_context.Stock.Remove(stock);

			return _context.SaveChangesAsync();
		}

		public Task<int> UpdateStockRange(List<Stock> stockList)
		{

			_context.UpdateRange(stockList);

			return _context.SaveChangesAsync();
		}

		public Stock GetStockWithProduct(int stockId)
		{
			return _context.Stock
	            .Include(x => x.Product)
	            .FirstOrDefault(x => x.Id == stockId);
		}

		public Task PutStockOnHold(int stockId, int qty, string sessionId)
		{
            _context.Stock.FirstOrDefault(x => x.Id == stockId).Quantity -= qty;

			var stockOnHold = _context.StockOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();

			if (stockOnHold.Any(x => x.StockId == stockId))
			{
				stockOnHold.Find(x => x.StockId == stockId).Quantity += qty;
			}
			else
			{
				_context.StockOnHold.Add(new StockOnHold
				{
					StockId = stockId,
					SessionId = sessionId,
					Quantity = qty,
					ExpiryDate = System.DateTime.Now.AddMinutes(20)
				});
			}

			foreach (var stock in stockOnHold)
			{
				stock.ExpiryDate = DateTime.Now.AddMinutes(20);
			}

			return _context.SaveChangesAsync();

		}

		public Task RemoveStockFromHold(string sessionId)
		{
			var stockOnHold = _context.StockOnHold
				.Where(x => x.SessionId == sessionId)
				.ToList();

			_context.StockOnHold.RemoveRange(stockOnHold);

			return _context.SaveChangesAsync();
		}

		public Task RemoveStockFromHold(int stockId, int qty, string sessionId)
		{
			var stockOnHold = _context.StockOnHold
			.FirstOrDefault(x => x.StockId == stockId
							&& x.SessionId == sessionId);

			var stock = _context.Stock.FirstOrDefault(x => x.Id == stockId);

			stock.Quantity += qty;
			stockOnHold.Quantity -= qty;

			if (stockOnHold.Quantity <= 0)
				_context.StockOnHold.Remove(stockOnHold);

			return _context.SaveChangesAsync();
		}

		public Task RetriveExpiredStockOnHold()
		{
			var stocksOnHold = _context.StockOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

			if (stocksOnHold.Count > 0)
			{
				var stockIdsOnHold = stocksOnHold
					.Select(x => x.StockId)
					.ToList();

				var stockToReturn = _context.Stock
					.Where(x => stockIdsOnHold.Contains(x.Id))
					.ToList();

				foreach (var stock in stockToReturn)
				{
					stock.Quantity = stock.Quantity + stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Quantity;
				}

				_context.StockOnHold.RemoveRange(stocksOnHold);

				return _context.SaveChangesAsync();
			}

			return Task.CompletedTask;
		}
	}
}
