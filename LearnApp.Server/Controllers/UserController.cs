using Microsoft.AspNetCore.Mvc;

namespace LearnApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> logger;

		public UserController(ILogger<UserController> logger)
		{
			this.logger = @logger;
		}

		[HttpPost("/user/create")]
		public IActionResult CreateUser()
		{
			return Ok();
		}


		[HttpGet("/user/get/{id}")]
		public IActionResult Get(int id)
		{
			logger.LogInformation("User requested");

			return Ok(new User()
				{
					address = new Address
					{
						street = "Neuer Weg",
						number = "1a",
						city = "Norden",
						postalCode = 26506,
						country = "Germany"
					},
					name = new Name
					{
						first = "Julian",
						last = "Bents",
						display = "Julian Bents"
					},
					contact = new Contact
					{
						phone = "+49 123 456789",
						mail = "test@gmail.com"
					},
					id = id
				}
			);
		}
	}
}
