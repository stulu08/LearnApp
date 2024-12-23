using LearnApp.Server.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LearnApp.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LessonController : ControllerBase
	{
		private readonly ILogger<UserController> logger;
		private readonly DBContext.UserContext lessonContext;

		public LessonController(ILogger<UserController> logger, DBContext.UserContext context)
		{
			this.logger = @logger;
			this.lessonContext = context;
		}

		[HttpGet("get/{id}")]
		public IActionResult Get(int id)
		{
			Lesson? lesson = lessonContext.GetLesson(id);
			if (lesson == null) {
				return Conflict("Lesson with id does not exist!");
			}

			return Ok(lesson);
		}

		[HttpGet("fetch/{id}")]
		public IActionResult FetchWithUser(int id)
		{
			Lesson? lesson = lessonContext.GetLesson(id);
			if (lesson == null)
				return Conflict("Lesson with id does not exist!");

			User? user = lessonContext.GetUser(lesson.user);
			if (user == null)
				return Conflict("Invalid Lesson!");

			UserStats? stats = lessonContext.GetUserStats(lesson.user);
			if (stats == null)
				return Conflict("Invalid Lesson!");


			lessonContext.MakeUserPublic(user);

			return Ok(new { lesson, user, stats });
		}
		
		[HttpGet("suggest/{userID}/{offset}/{limit}")]
		public IActionResult Suggest(int limit, int offset, int userID) {
			//User? user = lessonContext.GetUser(userID);

			int lessonCount = lessonContext.Lessons.Count();

			List<Lesson> lessons = [.. lessonContext.Lessons.
				Skip(offset).
				Take(Math.Min(limit, lessonCount)).
				Select(l => l)];

			return Ok(lessons);
		}
		
		[HttpGet("search/{query}/{userID}/{offset}/{limit}")]
		public IActionResult Search(string query, int limit, int offset, int userID) {
			//User? user = lessonContext.GetUser(userID);

			int lessonCount = lessonContext.Lessons.Count();
			
			if (string.IsNullOrEmpty(query)) {
				return Suggest(limit, offset, userID);
			}

			query = query.ToLower();

			List<Lesson> lessons = [.. lessonContext.Lessons.
				Where(l => 
						(l.titel != null && l.titel.ToLower().Contains(query)) ||
						(l.tags != null && l.tags.Any(tag => tag.ToLower().Contains(query)))).
				Skip(offset).
				Take(Math.Min(limit, lessonCount)).
				Select(l => l)];

			return Ok(lessons);
		}

		[HttpGet("user/{userID}/{offset}/{limit}")]
		public IActionResult GetUsers(int limit, int offset, int userID)
		{
			List<Lesson> lessons = [.. lessonContext.Lessons
				.Where(l => l.user == userID)
				.Skip(offset).Take(limit)
				.Select(l => l)];

			return Ok(lessons);
		}

		[HttpPost("create/{password}")]
		public IActionResult Create([FromBody] Lesson @base, string password)
		{
			User? user = lessonContext.GetUser(@base.user);
			if (user == null || !lessonContext.CheckUser(user, password))
				return Conflict("User not found");

			string tempImagePath = TempThumbnail(user.id);
			if (!System.IO.File.Exists(tempImagePath)) {
				return Conflict("No thumbnail provided!");
			}

			@base.rating = 0;

			EntityEntry<Lesson> lessonEntry = lessonContext.Lessons.Add(@base);
			lessonContext.SaveChanges();

			Lesson lesson = lessonEntry.Entity;

			System.IO.File.Move(tempImagePath, Thumbnail(lesson.id), true);

			return CreatedAtAction(nameof(Get), new { lesson.id }, lesson);
		}
		
		[HttpPost("delete/{id}/{userID}/{password}")]
		public IActionResult Delete(int id, int userID, string password)
		{
			User? user = lessonContext.GetUser(userID);
			if (user == null || !lessonContext.CheckUser(user, password))
				return Conflict("User not found");

			Lesson? lesson = lessonContext.GetLesson(id);
			if (lesson == null)
				return Conflict("Lesson not found");

			lessonContext.Lessons.Remove(lesson);
			lessonContext.SaveChanges();

			return Ok();
		}

		[HttpGet("thumbnail/get/{id}")]
		public IActionResult GetThumbnail(int id)
		{
			string filePath = Thumbnail(id);
			if (!System.IO.File.Exists(filePath))
			{
				return Conflict($"No thumbnail found for lesson {id}!");
			}

			var image = System.IO.File.OpenRead(filePath);
			return File(image, "image/jpeg");
		}
		
		[HttpPost("thumbnail/push/{userID}/{password}")]
		public async Task<IActionResult> PushThumbnail([FromForm] IFormFile file, int userID, string password)
		{
			if (file == null)
				return BadRequest("No file uploaded!");
			
			User? user = lessonContext.GetUser(userID);
			if (user == null || !lessonContext.CheckUser(user, password))
				return Conflict("User not found");

			string filePath = TempThumbnail(userID);
			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return Ok(new ImageUploadResult() { path = filePath });
		}
		
		[HttpGet("thumbnail/temp/{userID}")]
		public IActionResult GetTempThumbnail(int userID)
		{
			string filePath = TempThumbnail(userID);
			if (!System.IO.File.Exists(filePath))
			{
				return Conflict($"No temp thumbnail found for user {userID}!");
			}
			var image = System.IO.File.OpenRead(filePath);
			return File(image, "image/jpeg");
		}

		public string TempThumbnail(int userID)
		{
			string dir = Path.Combine("Data", "Lesson", "New", userID.ToString());
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			
			return Path.Combine(dir, "thumb");
		}
		public string Thumbnail(int lessonID)
		{
			string dir = Path.Combine("Data", "Lesson", lessonID.ToString());
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			return Path.Combine(dir, "thumb");
		}
	}
}
