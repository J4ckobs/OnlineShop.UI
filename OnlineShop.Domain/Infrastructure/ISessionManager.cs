using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;


namespace OnlineShop.Domain.Infrastructure
{
	public interface ISessionManager
	{
		string GetId();

		void AddProduct(CartProduct cartProduct);

		void RemoveProduct(int stockId, int qty);

		void ClearCart();

		IEnumerable<TResult> GetCart<TResult>(Func<CartProduct,TResult> selector);

		void AddCustomerInformation(CustomerInformation customer);

		CustomerInformation GetCustomerInformation();
    }
}
