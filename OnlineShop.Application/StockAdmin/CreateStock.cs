using OnlineShop.Database;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.StockAdmin
{
    public class CreateStock
    {
        public ApplicationDbContext _context;
        public CreateStock(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock
            {
				ProductId = request.ProductId,
				Description = request.Description,
                Quantity = request.Quantity
            };
            
            _context.Add(stock);

            await _context.SaveChangesAsync();

            return new Response
            {
                Id = stock.Id,
                Description = request.Description,
                Quantity = request.Quantity
            };
        }

        public class Request
        {
            public int ProductId { get; set; }
			public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
		}

        public class Response
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }
    }
}
