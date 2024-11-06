using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Application.UserAdmin
{
	public class DeleteUser
	{
		private UserManager<IdentityUser> _userManager;

		public DeleteUser(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}


		public async Task<bool> DoAsync(string id)
		{
			var User = _userManager.Users.FirstOrDefault(x => x.Id == id);

			if (User == null)
				return false;

			await _userManager.DeleteAsync(User);

			return true;
		}
	}
}
