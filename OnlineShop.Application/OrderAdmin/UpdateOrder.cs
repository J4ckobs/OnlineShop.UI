﻿using OnlineShop.Domain.Infrastructure;

namespace OnlineShop.Application.OrderAdmin
{
	[Service]
	public class UpdateOrder
    {
		private IOrderManager _orderManager;

		public UpdateOrder(IOrderManager orderManager)
        {
			_orderManager = orderManager;
        }

        public Task<int> DoAsync(int id)
        {
            return _orderManager.AdvanceOrder(id);
        }
    }
}
