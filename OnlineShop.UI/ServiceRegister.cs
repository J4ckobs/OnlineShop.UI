using OnlineShop.Application;
using OnlineShop.Database;
using OnlineShop.Domain.Infrastructure;
using OnlineShop.UI.Infrastructure;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceRegister
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
		{
			var servicesType = typeof(Service);
			var definedTypes = servicesType.Assembly.DefinedTypes;


			var services = definedTypes
				.Where(x => x.GetTypeInfo().GetCustomAttribute<Service>() != null);

			foreach (var service in services)
			{
				@this.AddTransient(service);
			}

			@this.AddTransient<IOrderManager, OrderManager>();
			@this.AddTransient<IProductManager, ProductManager>();
			@this.AddTransient<IStockManager, StockManager>();
			//@this.AddTransient<IUserManager, UserManager>();

			@this.AddScoped<ISessionManager, SessionManager>();


			return @this;
		}
	}
}
