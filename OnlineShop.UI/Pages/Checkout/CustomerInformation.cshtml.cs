using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;
using OnlineShop.Database;

namespace OnlineShop.UI.Pages.Checkout
{
    public class CustomerInformationModel : PageModel
    {
		private IWebHostEnvironment _env;

		public CustomerInformationModel(IWebHostEnvironment env)
        {
            _env = env;
        }


        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }

        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
            {
                if(_env.IsDevelopment())
                {
                    CustomerInformation = new AddCustomerInformation.Request
                    {
                        FirstName = "a",
                        LastName = "b",
                        Email = "abs@a.com",
                        PhoneNumber = "123456789",

                        Address1 = "e",
                        Address2 = "f",
                        City = "g",
                        PostCode = "12-234"
                    };
                }
                return Page();
            }
            else
                return RedirectToPage("/Checkout/Payment");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            new AddCustomerInformation(HttpContext.Session).Do(CustomerInformation);

            return RedirectToPage("/Checkout/Payment");
        }
    }
}
