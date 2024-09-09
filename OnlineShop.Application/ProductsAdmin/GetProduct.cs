using OnlineShop.Database;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.ProductsAdmin
{
    public class GetProduct
    {
        private ApplicationDbContext _context;
        public GetProduct(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public ProductViewModel Do(int id) =>
            _context.Products.Where(x => x.Id == id).Select(x => new ProductViewModel {
                Id = x.Id,
                Name = x.Name,
				Description = x.Description,
                Value = x.Value
            })
            .FirstOrDefault();

        public class ProductViewModel
        {
            public int Id { get; set; } = 0;
            public string Name { get; set; } = "";
			public string Description { get; set; } = "";
            public decimal Value { get; set; } = 0;
        }
    }
}
