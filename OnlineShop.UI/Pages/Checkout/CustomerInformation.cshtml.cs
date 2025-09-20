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

        public IActionResult OnGet(
            [FromServices] GetCustomerInformation getCustomerInformation)
        {
            var information = getCustomerInformation.Do();

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

        public IActionResult OnPost(
            [FromServices] AddCustomerInformation addCustomerInformation)
        {
            if (!ModelState.IsValid)
                return Page();

            addCustomerInformation.Do(CustomerInformation);

            return RedirectToPage("/Checkout/Payment");
        }
    }
}
