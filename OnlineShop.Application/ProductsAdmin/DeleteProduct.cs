using OnlineShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.ProductsAdmin
{
    public class DeleteProduct
    {
        private ApplicationDbContext _context;
        public DeleteProduct(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<bool> Do(int id)
        {
            var Product = _context.Products.FirstOrDefault(x => x.Id == id);

            _context.Products.Remove(Product);

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
