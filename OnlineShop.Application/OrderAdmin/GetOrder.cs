using Microsoft.EntityFrameworkCore;
using OnlineShop.Database;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.OrderAdmin
{
    public class GetOrder
    {
        private ApplicationDbContext _context;

        public GetOrder(ApplicationDbContext context)
        {
            _context = context;
        }

        public Response Do(int id) =>
            _context.Orders
                .Where(x => x.Id == id)
                    .Select(order => new Response
            {
                        Id = order.Id,
                        OrderRef = order.OrderRef,
                        Email = order.Email
                    })?.FirstOrDefault() ?? new Response { }; // Is it correct?


        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }
    }
}

