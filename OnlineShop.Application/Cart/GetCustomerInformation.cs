using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineShop.Domain.Models;
using System.Text;

namespace OnlineShop.Application.Cart
{
	public class GetCustomerInformation
	{
		private ISession _session;
		public GetCustomerInformation(ISession session)
		{
			_session = session;
		}


		public class Request
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }

			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string PostCode { get; set; }
		}

		public Request Do()
		{
			var stringObject = _session.GetString("customer-info");

			if (String.IsNullOrEmpty(stringObject))
				return null;

			var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);

			return new Request
			{
				FirstName = customerInformation.FirstName,
				LastName = customerInformation.LastName,
				Email = customerInformation.Email,
				PhoneNumber = customerInformation.PhoneNumber,

				Address1 = customerInformation.Address1,
				Address2 = customerInformation.Address2,
				City = customerInformation.City,
				PostCode = customerInformation.PostCode,
			};
		}
	}
}
