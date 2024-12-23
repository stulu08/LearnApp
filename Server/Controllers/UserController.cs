using LearnApp.Server.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LearnApp.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> logger;
		private readonly DBContext.UserContext userContext;

		public UserController(ILogger<UserController> logger, DBContext.UserContext context)
		{
			this.logger = @logger;
			this.userContext = context;
		}

		[HttpPost("create/{password}")]
		public IActionResult CreateUser([FromBody] UserBase @base, string password) {
			if (@base == null) {
				return BadRequest("No User data uploaded.");
			}
			if (!@base.valid()) {
				return BadRequest("Invalid User data uploaded.");
			}

			User? baseUser = Server.User.CreateFromBase(@base);
			if (baseUser == null) {
				return Conflict("Wrong data provided, user could not be created!");
			}
			baseUser.SetPassword(password);
			
			if (userContext.Users.Select(u => u.mail).Where(mail => mail == @base.mail).Count() > 0)
			{
				return Conflict($"Account with mail '{@base.mail}' already exists!");
			}

			EntityEntry<User> userObj = userContext.Users.Add(baseUser);
			userContext.SaveChanges();

			User user = userObj.Entity;

			return CreatedAtAction(
			   nameof(Get),
			   new { user.id, password },
			   user 
			 );
		}

		[HttpGet("get/{id}/{password}")]
		public IActionResult Get(int id, string password)
		{
			User? user = userContext.GetUser(id);
			if (user == null || !userContext.CheckUser(user, password)) {
				return Conflict("Account not found!");
			}

			return Ok(user);
		}

		[HttpGet("fetch/{id}")]
		public IActionResult Fetch(int id)
		{
			User? user = userContext.GetUser(id);
			if (user == null) {
				return Conflict("Account not found!");
			}

			userContext.MakeUserPublic(user);
			UserStats stats = userContext.GetUserStats(id);

			return Ok(new { user, stats });
		}

		[HttpGet("find/{mail}/{password}")]
		public IActionResult FindByMail(string mail, string password)
		{
			logger.LogInformation("User requested by mail");

			List<User> users = [.. userContext.Users
 				.Where(user => user.mail == mail)
				.Select(user => user )];

			if (users.Count != 1)
				return Conflict("Account not found!");

			User user = users[0];

			if (!userContext.CheckUser(user, password)) {
				return Conflict("Account not found!");
			}

			return Ok(user);
		}

		[HttpGet("stats/{id}")]
		public IActionResult GetStats(int id) {
			User? user = userContext.GetUser(id);
			if (user == null)
				return Conflict("Account not found!");
			
			return Ok(userContext.GetUserStats(id));
		}
		
		[HttpGet("public/{id}")]
		public IActionResult GetPublic(int id) {
			User? user = userContext.GetUser(id);
			if (user == null)
				return Conflict("Account not found!");

			userContext.MakeUserPublic(user);

			return Ok(user);
		}
	}
}
