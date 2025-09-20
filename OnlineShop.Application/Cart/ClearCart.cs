using OnlineShop.Domain.Infrastructure;

namespace OnlineShop.Application.Cart
{
	[Service]
	public class ClearCart
	{
		private ISessionManager _sessionManager;

		public ClearCart(ISessionManager sessionManager)
        {
			_sessionManager = sessionManager;
        }

		public void Do()
		{
			_sessionManager.ClearCart();
		}
    }
}
