using OnlineShop.Domain.Enums;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Infrastructure
{
	public interface IOrderManager
	{
		bool OrderReferenceExists(string reference);


		IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector);

		TResult GetOrderById<TResult>(int id, Expression<Func<Order, TResult>> selector);
		TResult GetOrderByReference<TResult>(string reference, Expression<Func<Order, TResult>> selector);

		Task<int> CreateOrder(Order order);
		Task<int> AdvanceOrder(int id);
	}
}
