using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Enums;
using OnlineShop.Domain.Infrastructure;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShop.Database
{
	public class OrderManager : IOrderManager
	{
		private readonly ApplicationDbContext _context;

        public OrderManager(ApplicationDbContext context)
        {
            _context = context;
        }

		public bool OrderReferenceExists(string reference)
		{
			return _context.Orders.Any(x => x.OrderRef == reference);
		}

		public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
		{
			return _context.Orders
				.Where(x => x.Status == status)
					.Select(selector).ToList();
		}

		private TResult GetOrder <TResult>(
			Expression<Func<Order, bool>> condition,
			Expression<Func<Order, TResult>> selector)
		{
			return _context.Orders
				.Where(condition)
				.Include(x => x.OrderStocks)
					.ThenInclude(x => x.Stock)
						.ThenInclude(x => x.Product)
				.Select(selector)
				.FirstOrDefault();
		}

		public TResult GetOrderById<TResult>(int id, Expression<Func<Order, TResult>> selector)
		{
			return GetOrder(order => order.Id == id, selector);
		}

		public TResult GetOrderByReference<TResult>(
			string reference,
			Expression<Func<Order,TResult>> selector)  
		{
			return GetOrder(order => order.OrderRef == reference, selector);
		}

		public Task<int> CreateOrder(Order order)
		{
			_context.Orders.Add(order);

			return _context.SaveChangesAsync();
		}

		public Task<int> AdvanceOrder(int id)
		{
			_context.Orders.FirstOrDefault(x => x.Id == id).Status++;

			return _context.SaveChangesAsync();
		}
	}
}
