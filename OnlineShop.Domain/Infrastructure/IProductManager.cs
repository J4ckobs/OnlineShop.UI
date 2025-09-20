using OnlineShop.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Infrastructure
{
	public interface IProductManager
	{
		Task<int> CreateProduct(Product product);
		Task<int> DeleteProduct(int Id);
		Task<int> UpdateProduct(Product product);

		//TResult GetProductById<TResult>(int id, Func<Product, TResult> selector);
		TResult GetProductById<TResult>(int id, Func<Product, TResult> selector);
		TResult GetProductByName<TResult>(string name, Func<Product, TResult> selector);

		IEnumerable<TResult> GetProductsWithStock<TResult>(Func<Product, TResult> selector);

	}
}
