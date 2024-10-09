using OnlineShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Products
{
    public class GetProducts
    {
        private ApplicationDbContext _context;
        public GetProducts(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _context.Products.ToList().Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = $"{x.Value.ToString("N2")} $" // 1100.50 -> 1,100.50 -> 1,100.50 $
            });

        public class ProductViewModel
        {
			public string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public string Value { get; set; }
        }
    }
}
