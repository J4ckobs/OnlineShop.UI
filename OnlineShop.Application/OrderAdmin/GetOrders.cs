using OnlineShop.Database;
using OnlineShop.Domain.Enums;

namespace OnlineShop.Application.OrderAdmin
{
    public class GetOrders
    {
        private ApplicationDbContext _context;

        public GetOrders(ApplicationDbContext context)
        {
            _context = context;
        }

		public class Response
		{
			public int Id { get; set; }
			public string OrderRef { get; set; }
			public string Email { get; set; }
		}

		public IEnumerable<Response> Do(int status) =>
			_context.Orders
				.Where(x => x.Status == (OrderStatus)status)
					.Select(order => new Response
					{
						Id = order.Id,
						OrderRef = order.OrderRef,
						Email = order.Email,
					}).ToList();
	}
}
