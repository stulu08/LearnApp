using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LearnApp.Server
{
	public class Utils
	{
		public static bool StringValid(string? str, int bound = 32, int lowerBound = 3)
		{
			if (str == null)
				return false;
			return str.Length < bound && str.Length > lowerBound;
		}
	}

	public class Name
	{
		public string? display { get; set; }
		public string? first { get; set; }
		public string? last { get; set; }

		public bool valid()
		{
			return Utils.StringValid(display) && Utils.StringValid(first) && Utils.StringValid(last);
		}
	}
	public class Address
	{
		public string? street { get; set; }
		public string? number { get; set; }
		public string? city { get; set; }
		public string? country { get; set; }
		public int postalCode { get; set; }

		public bool valid()
		{
			return Utils.StringValid(street, 50) && Utils.StringValid(number, 5)
				&& Utils.StringValid(city) && Utils.StringValid(country) 
				&& postalCode != 0;
		}
	}
	public class Contact
	{
		public string? phone { get; set; }
		public string? mail { get; set; }

		public bool valid()
		{
			return Utils.StringValid(phone, 15, 8) && Utils.StringValid(mail);
		}
	}
	public class UserBase
	{
		public required Name name { get; set; }
		public required Address address { get; set; }
		public required Contact contact { get; set; }

		public bool valid()
		{
			if (name == null || address == null || contact == null)
				return false;
			return name.valid() && address.valid() && contact.valid();
		}

	}
	public class User : UserBase
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public static User? CreateFromBase(UserBase @base) 
		{
			if (@base == null) {
				return null;
			}
			if (!@base.valid()) {
				return null;
			}

			return new User()
			{
				name = @base.name,
				address = @base.address,
				contact = @base.contact,
				id = System.Random.Shared.Next()
			};
			;
		}
	}
}
