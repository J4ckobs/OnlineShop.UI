using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.UserAdmin
{
	public class GetUsers
	{
		private UserManager<IdentityUser> _userManager;

		public GetUsers(UserManager<IdentityUser> userManager)
        {
			_userManager = userManager;
        }


		public IEnumerable<ProductViewModel> Do()
		{
			return _userManager.Users.Select(x => new ProductViewModel
			{
				Id = x.Id,
				UserName = x.UserName,
			}).ToList();
		}

		public class ProductViewModel
		{
            public string UserName { get; set; }
			public string Id { get; set; }

			public IEnumerable<string> Roles { get; set; }
		}
	}
}
