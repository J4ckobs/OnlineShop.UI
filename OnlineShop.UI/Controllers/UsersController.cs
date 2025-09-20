using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.UI.ViewModels.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class ProductViewModel
		{
            public string? Username { get; set; }
			public string? Id { get; set; }
			public string? Role { get; set; }
		}

        [HttpPost("")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel vm)
        {
			var managerUser = new IdentityUser()
			{
				UserName = vm.Username
			};

			await _userManager.CreateAsync(managerUser, vm.Password);

			var managerClaim = new Claim("Role", "Manager");

			await _userManager.AddClaimAsync(managerUser, managerClaim);

			return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
			var usersQuery = _userManager.Users.ToList();
			var users = new List<ProductViewModel>();

			foreach(var user in usersQuery)
			{
				var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
				users.Add(new ProductViewModel
				{
					Id = user.Id,
					Username = user.UserName,
					Role = role
				});
			}

			return Ok(users);
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
			var User = _userManager.Users.FirstOrDefault(x => x.Id == id);

			if (User != null)
				return Ok(await _userManager.DeleteAsync(User));

			return Ok("User not found");

        }
	}
}
