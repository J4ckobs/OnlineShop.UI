﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Models
{
	public class CustomerInformation
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
}
