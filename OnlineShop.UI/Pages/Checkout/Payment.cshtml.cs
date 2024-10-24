using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;
using OnlineShop.Application.Orders;
using OnlineShop.Database;
using Stripe;
using Stripe.Checkout;
using static OnlineShop.Application.Cart.GetCustomerInformation;

namespace OnlineShop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
		private ApplicationDbContext _context;

		public string SessionId { get; set; }

		public Request customerInformation { get; set; }

		public PaymentModel(IConfiguration config, ApplicationDbContext context)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"].ToString();
			_context = context;
		}

		public IActionResult OnGet()
        {
			customerInformation = new GetCustomerInformation(HttpContext.Session).Do();

			if (customerInformation == null)
				return RedirectToPage("/Checkout/CustomerInformation");

            return Page();
		}

		public async Task<IActionResult> OnPost(string stripeEmail, string stripeToken)
		{
			var currency = "pln";
			var successUrl = Url.PageLink("Success"); //"https://localhost:7020/Success";
			var cancelUrl = "https://localhost:7020/";

			var customers = new CustomerService();

			var CartOrder = new Application.Cart.GetOrder(HttpContext.Session,_context).Do();

			if (CartOrder.Products == null || CartOrder.CustomerInformation == null)
				return RedirectToPage("/Index");


			var productsIds = CartOrder.Products.Select(x => x.StockId).ToList();
			var products = _context.Stock.Where(x => productsIds.Contains(x.Id)).ToList();


			var customer = customers.Create(new CustomerCreateOptions
			{
				Email = stripeEmail,
				Source = stripeToken
			});

			var stripePaymentOptions = new SessionCreateOptions
			{
				Customer = customer.Id,
				CustomerEmail = customer.Email,

				PaymentMethodTypes = new List<string>
				{
					"card"
				},

				AllowPromotionCodes = false,

				LineItems = CartOrder.Products
					.Select(x => new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = currency,
							UnitAmount = x.Value,
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = products.FirstOrDefault(y => x.StockId == y.Id)?.Product.Name
							}
						},
						Quantity = x.Quantity
					}).ToList(),

				Mode = "payment",
				SuccessUrl = successUrl,
				CancelUrl = cancelUrl
			};

			var service = new SessionService();
			var session = service.Create(stripePaymentOptions);
			SessionId = session.Id;

			var sessionId = HttpContext.Session.Id;

			await new CreateOrder(_context).Do(new CreateOrder.Request
			{
				StripeReference = customer.Id,
				SessionId = sessionId,

				FirstName = CartOrder.CustomerInformation.FirstName,
				LastName = CartOrder.CustomerInformation.LastName,
				Email = CartOrder.CustomerInformation.Email,
				PhoneNumber = CartOrder.CustomerInformation.PhoneNumber,

				Address1 = CartOrder.CustomerInformation.Address1,
				Address2 = CartOrder.CustomerInformation.Address2,
				City = CartOrder.CustomerInformation.City,
				PostCode = CartOrder.CustomerInformation.PostCode,

				Stocks = CartOrder.Products.Select( x => new CreateOrder.Stock
				{
					StockId = x.StockId,
					Quantity = x.Quantity,
				}).ToList()
			});

			return Redirect(session.Url);
		}
    }
}
