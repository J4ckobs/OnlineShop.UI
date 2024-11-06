using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ProductsAdmin;
using OnlineShop.Application.StockAdmin;
using OnlineShop.Application.UserAdmin;
using OnlineShop.Database;

namespace OnlineShop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class UsersController : Controller
    {
        private CreateUser _createUser;
        private UserManager<IdentityUser> _getUsers;

        public UsersController(CreateUser createUser, UserManager<IdentityUser> getUsers)
        {
            _createUser = createUser;
            _getUsers = getUsers;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser.Request request)
        {
            await _createUser.Do(request);

            return Ok();
        }

        [HttpGet("")]
        public IActionResult GetUsers() => Ok(new GetUsers(_getUsers).Do());

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id) => Ok(await new DeleteUser(_getUsers).DoAsync(id));
	}
}
