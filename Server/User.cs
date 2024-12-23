using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

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
		public static readonly int SaltLength = 16;
		public static string PasswordHash(string pwd)
		{
			byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);
			return PasswordHash(pwd, salt);
		}
		public static string PasswordHash(string pwd, byte[] salt) {
			byte[] hash = KeyDerivation.Pbkdf2(
				password: pwd!,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 32);
			
			byte[] saltAndHash = new byte[salt.Length + hash.Length];
			Buffer.BlockCopy(salt, 0, saltAndHash, 0, salt.Length);
			Buffer.BlockCopy(hash, 0, saltAndHash, salt.Length, hash.Length);

			return Convert.ToBase64String(saltAndHash);

		}
		public static bool PasswordVerify(string? sourceSaltHash, string? pwd)
		{
			if(sourceSaltHash == null || pwd == null) return false;

			byte[] storedSaltAndHash = Convert.FromBase64String(sourceSaltHash);

			byte[] salt = new byte[SaltLength];
			Buffer.BlockCopy(storedSaltAndHash, 0, salt, 0, salt.Length);

			byte[] storedHash = new byte[storedSaltAndHash.Length - salt.Length];
			Buffer.BlockCopy(storedSaltAndHash, salt.Length, storedHash, 0, storedHash.Length);

			byte[] targetHash = KeyDerivation.Pbkdf2(
				password: pwd,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 32);

			return targetHash.SequenceEqual(storedHash);
		}
	}

	[Owned]
	public class Name
	{
		[Length(3, 32)]
		public string? display { get; set; }
		[Length(3, 32)]
		public string? first { get; set; }
		[Length(3, 32)]
		public string? last { get; set; }

		public bool valid()
		{
			return Utils.StringValid(display) && Utils.StringValid(first) && Utils.StringValid(last);
		}
	}
	[Owned]
	public class Address
	{
		[Length(3, 50)]
		public string? street { get; set; }
		[Length(3, 32)]
		public string? city { get; set; }
		[Length(3, 32)]
		public string? country { get; set; }
		[Length(3, 32)]
		public string? postalCode { get; set; }

		public bool valid()
		{
			return Utils.StringValid(street, 50)
				&& Utils.StringValid(city) 
				&& Utils.StringValid(country) 
				&& Utils.StringValid(postalCode);
		}
	}
	public class UserBase
	{
		[Required]
		public required Name name { get; set; }
		public required Address address { get; set; }

		[Required, EmailAddress, Length(6, 50)]
		public string? mail { get; set; }

		public bool valid()
		{
			if (name == null || address == null || mail == null)
				return false;
			return name.valid() && address.valid() && Utils.StringValid(mail, 50, 6);
		}

	}
	public class User : UserBase
	{

		[Required, Base64String]
		public string? password { get; set; }
		[Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
				mail = @base.mail,
			};
			;
		}

		public void SetPassword(string pwd)
		{
			this.password = Utils.PasswordHash(pwd);
		}
		public bool ChechPassword(string pwd)
		{
			if(this.password == null)
				return false;

			return Utils.PasswordVerify(this.password, pwd);
		}
	}

	public class UserStats
	{
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int userID;
		public int rating { get; set; }
		public int ratingCount { get; set; }
		public string? description { get; set; }
		public string? headline { get; set; }
		[Url]
		public string? avatarURL { get; set; }
	}
}
