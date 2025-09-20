using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Application.Cart;
using OnlineShop.Application.Orders;
using Stripe;
using static OnlineShop.Application.Cart.GetCustomerInformation;
using GetOrderCart = OnlineShop.Application.Cart.GetOrder;

namespace OnlineShop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
		public string SessionId { get; set; }

		public Request customerInformation { get; set; }

		public PaymentModel(IConfiguration config)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"].ToString();
		}

		public IActionResult OnGet(
			[FromServices] GetCustomerInformation getCustomerInformation)
        {
			customerInformation = getCustomerInformation.Do();

			if (customerInformation == null)
				return RedirectToPage("/Checkout/CustomerInformation");

            return Page();
		}

		public async Task<IActionResult> OnPost(
			string stripeEmail,
			string stripeToken,
			[FromServices] GetOrderCart getOrder,
			[FromServices] CreateOrder createOrder)
		{
			var currency = "pln";
			var successUrl = Url.PageLink("Success"); //"https://localhost:7020/Success";
			var cancelUrl = "https://localhost:7020/";

			var customers = new CustomerService();
			var charges = new ChargeService();

			var cartOrder = getOrder.Do();

			if (cartOrder.Products == null || cartOrder.CustomerInformation == null)
				return RedirectToPage("/Index");

			//var productsIds = cartOrder.Products.Select(x => x.StockId).ToList();
			//var products = _context.Stock.Where(x => productsIds.Contains(x.Id)).ToList();

			var customer = customers.Create(new CustomerCreateOptions
			{
				Email = stripeEmail,
				Source = stripeToken
			});

			var charge = charges.Create(new ChargeCreateOptions
			{
				Amount = cartOrder.GetTotalCharge(),
				Description = "Shop Purchase",
				Currency = currency,
				Customer = customer.Id
			});

			/*
			var stripePaymentOptions = new SessionCreateOptions
			{
				Customer = customer.Id,
				CustomerEmail = customer.Email,

				PaymentMethodTypes = new List<string>
				{
					"card"
				},

				AllowPromotionCodes = false,

				LineItems = cartOrder.Products
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
			*/

			var sessionId = HttpContext.Session.Id;

			await createOrder.Do(new CreateOrder.Request
			{
				StripeReference = customer.Id,
				SessionId = sessionId,

				FirstName = cartOrder.CustomerInformation.FirstName,
				LastName = cartOrder.CustomerInformation.LastName,
				Email = cartOrder.CustomerInformation.Email,
				PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,

				Address1 = cartOrder.CustomerInformation.Address1,
				Address2 = cartOrder.CustomerInformation.Address2,
				City = cartOrder.CustomerInformation.City,
				PostCode = cartOrder.CustomerInformation.PostCode,

				Stocks = cartOrder.Products.Select( x => new CreateOrder.Stock
				{
					StockId = x.StockId,
					Quantity = x.Quantity,
				}).ToList()
			});

			//return Redirect(session.Url);
			return RedirectToPage("/Index");
		}
    }
}
