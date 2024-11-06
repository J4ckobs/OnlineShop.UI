using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.OrderAdmin;
using OnlineShop.Database;

namespace OnlineShop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        //Orders
        [HttpGet("")]
        public IActionResult GetOrders(
            int status,
            [FromServices] GetOrders getOrders) => 
                Ok(getOrders.Do(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(
            int id,
            [FromServices] GetOrder getOrder) => 
                Ok(getOrder.Do(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int id,
            [FromServices] UpdateOrder updateOrder) => 
                Ok(await updateOrder.DoAsync(id));

    }
}