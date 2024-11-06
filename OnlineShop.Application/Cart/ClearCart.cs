using Microsoft.AspNetCore.Http;
using OnlineShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Cart
{
	public class ClearCart
	{
		private ISession _session;

		public ClearCart(ISession session)
        {
			_session = session;
        }

		public void Do()
		{
			_session.Remove("cart");
		}
    }
}
