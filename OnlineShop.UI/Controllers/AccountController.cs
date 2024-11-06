using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace OnlineShop.UI.Controllers
{
	[Route("Accounts")]
	public class AccountController : Controller
	{
		private SignInManager<IdentityUser> _signInManager;

		public AccountController(SignInManager<IdentityUser> signInManager) 
		{
			_signInManager = signInManager;
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToPage("/Index");
		}
	}
}