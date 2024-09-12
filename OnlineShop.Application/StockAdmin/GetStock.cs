using Microsoft.EntityFrameworkCore;
using OnlineShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.StockAdmin
{
    public class GetStock
    {
        public ApplicationDbContext _context;
        public GetStock(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            var stock = _context.Products
                .Include(x => x.Stock)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Quantity = y.Quantity
                    })
                })
                .ToList();

            return stock;
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public IEnumerable<StockViewModel> Stock { get; set; } = Enumerable.Empty<StockViewModel>();
        }
    }
}
