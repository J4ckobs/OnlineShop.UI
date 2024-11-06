using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.UI.Pages.Accounts
{
    public class LoginModel : PageModel
    {
		private SignInManager<IdentityUser> _signInManager;

		public LoginModel(SignInManager<IdentityUser> singInManager)
        {
            _signInManager = singInManager;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if ((Input.Username != null) && (Input.Password != null))
            { }
            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, false);

            if (result.Succeeded)
            {
                Console.WriteLine("### Result Succeded ###");
                return RedirectToPage("/Admin/Index");
            }

            else
            {
                Console.WriteLine("### Result Not Succeded ### Name:" + Input.Username + "-Pass:" + Input.Password);


                return Page();
            }
            
        }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nazwa u¿ytkownika jest wymagana")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Has³o jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
