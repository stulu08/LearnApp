using LearnApp.Server.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

class Program
{
	public static Startup? startup;

	static void Main(string[] args) {
		startup = new Startup(args);
		startup.Run();
	}
}

internal class Startup
{
	private readonly WebApplicationBuilder builder;
	private readonly WebApplication app;
	public IConfiguration Configuration => builder.Configuration;

	public Startup(String[] argv)
	{
		builder = WebApplication.CreateBuilder(argv);
		builder.Configuration.AddEnvironmentVariables();

		builder.Services.AddControllers();

		if (UseMySQL())
		{
			string connectionString = GetConnectionString();

			ServerVersion version = ServerVersion.AutoDetect(connectionString);

			builder.Services.AddDbContext<UserContext>(options =>
				options.UseMySql(connectionString, version)
				.LogTo(Console.WriteLine, LogLevel.Information)
				.EnableSensitiveDataLogging()
				.EnableDetailedErrors()
				);
		}
		else
		{
			builder.Services.AddDbContext<UserContext>(options =>
				options.UseSqlServer(GetConnectionString()));
		}

		app = builder.Build();
		
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<UserContext>();
			db.Database.Migrate();
			if(db.GetUser(1) == null)
			{
				Console.WriteLine("Populating database with example data");
				if (db.Database.IsMySql())
				{
					db.Database.ExecuteSqlRaw("USE `LearnApp`;\r\n/* SET IDENTITY_INSERT [dbo].[Lessons] ON; */\r\n-- SQLINES FOR EVALUATION USE ONLY (14 DAYS)\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (7, 1, 'Angular Basics', 6, 'Learn the basics of Frontend web Development using Angular and Typescript!', '[\"Angular\",\"Computer Science \",\"Web Development \",\"Frontend\"]', 120, 55, 5);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (10, 1, 'Advanced German Grammar', 0, 'Master the advanced grammar of the German language.', '[\"Grammar\", \"Advanced\", \"German\"]', 60, 25, 3);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (11, 1, 'Conversational English', 1, 'Improve your conversational skills in English.', '[\"Speaking\", \"English\", \"Communication\"]', 45, 20, 2);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (12, 1, 'Algebra Foundations', 2, 'A comprehensive guide to the basics of algebra.', '[\"Math\", \"Algebra\", \"Basics\"]', 90, 30, 3);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (13, 1, 'Intro to Chemistry', 3, 'Learn the fundamentals of Chemistry.', '[\"Chemistry\", \"Basics\", \"Science\"]', 60, 25, 4);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (14, 1, 'Biology for Beginners', 4, 'Introduction to biology concepts.', '[\"Biology\", \"Introduction\", \"Science\"]', 60, 20, 2);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (15, 1, 'Physics for High School', 5, 'Understand the principles of physics.', '[\"Physics\", \"High School\", \"Fundamentals\"]', 75, 35, 2);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (16, 1, 'Learn Programming with C++', 6, 'Introduction to programming in C++.', '[\"Programming\", \"C++\", \"Computer Science\"]', 90, 40, 5);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (17, 1, 'World History Overview', 7, 'A detailed look at world history.', '[\"History\", \"World\", \"Overview\"]', 60, 30, 5);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (18, 1, 'Understanding Politics', 8, 'An overview of political systems and structures.', '[\"Politics\", \";vernment\", \"Introduction\"]', 60, 25, 2);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (19, 1, 'Chemistry Lab Basics', 3, 'A practical introduction to chemistry lab work.', '[\"Chemistry\", \"Lab\", \"Science\"]', 120, 50, 3);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (20, 1, 'Combining Angular with .NET Core', 6, '<p>Here you will learn the basics to create a simple web application using angular and a simple backend api with asp.net in c#!</p>', '[\"Angular\",\"Asp.Net\",\".Net\",\"C#\",\"Web Development\"]', 90, 45, 5);\r\nINSERT `Lessons` (`id`, `user`, `titel`, `lesson`, `description`, `tags`, `duration`, `price`, `rating`) VALUES (21, 1, 'Plattdeutsch Lernen', 0, '<p>Hier bring ich platt bei.</p>', '[\"Platt\",\"German\"]', 35, 20, 4);\r\n/* SET IDENTITY_INSERT [dbo].[Lessons] OFF; */\r\n\r\n/* SET IDENTITY_INSERT [dbo].[Users] ON; */\r\n\r\nINSERT `Users` (`id`, `password`, `name_display`, `name_first`, `name_last`, `address_street`, `address_city`, `address_country`, `address_postalCode`, `mail`) VALUES (1, 'HAPM2E2ASE9KL80pQ5IPmU+R9puh/dv0vE5eQ2Ty58arSzq65CgLTc08FxNtmihS', 'Stulu', 'Julian', 'Bents', 'Musterstra?e, 25', 'Norden', 'Deutschland', '26506', 'abc@gmail.com');\r\n\r\n/* SET IDENTITY_INSERT [dbo].[Users] OFF; */\r\n\r\nINSERT `UserStats` (`userID`, `rating`, `ratingCount`, `description`, `headline`, `avatarURL`) VALUES (1, 4, 28, 'Hello, this is my profile. I''m new here!', 'New User!', 'https://avatars.githubusercontent.com/u/58867625?v=4');\r\n");
				}
				else
					db.Database.ExecuteSqlRaw("USE [LearnApp];\r\nSET IDENTITY_INSERT [dbo].[Lessons] ON;\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (7, 1, N'Angular Basics', 6, N'Learn the basics of Frontend web Development using Angular and Typescript!', N'[\"Angular\",\"Computer Science \",\"Web Development \",\"Frontend\"]', 120, 55, 5);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (10, 1, N'Advanced German Grammar', 0, N'Master the advanced grammar of the German language.', N'[\"Grammar\", \"Advanced\", \"German\"]', 60, 25, 3);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (11, 1, N'Conversational English', 1, N'Improve your conversational skills in English.', N'[\"Speaking\", \"English\", \"Communication\"]', 45, 20, 2);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (12, 1, N'Algebra Foundations', 2, N'A comprehensive guide to the basics of algebra.', N'[\"Math\", \"Algebra\", \"Basics\"]', 90, 30, 3);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (13, 1, N'Intro to Chemistry', 3, N'Learn the fundamentals of Chemistry.', N'[\"Chemistry\", \"Basics\", \"Science\"]', 60, 25, 4);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (14, 1, N'Biology for Beginners', 4, N'Introduction to biology concepts.', N'[\"Biology\", \"Introduction\", \"Science\"]', 60, 20, 2);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (15, 1, N'Physics for High School', 5, N'Understand the principles of physics.', N'[\"Physics\", \"High School\", \"Fundamentals\"]', 75, 35, 2);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (16, 1, N'Learn Programming with C++', 6, N'Introduction to programming in C++.', N'[\"Programming\", \"C++\", \"Computer Science\"]', 90, 40, 5);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (17, 1, N'World History Overview', 7, N'A detailed look at world history.', N'[\"History\", \"World\", \"Overview\"]', 60, 30, 5);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (18, 1, N'Understanding Politics', 8, N'An overview of political systems and structures.', N'[\"Politics\", \";vernment\", \"Introduction\"]', 60, 25, 2);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (19, 1, N'Chemistry Lab Basics', 3, N'A practical introduction to chemistry lab work.', N'[\"Chemistry\", \"Lab\", \"Science\"]', 120, 50, 3);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (20, 1, N'Combining Angular with .NET Core', 6, N'<p>Here you will learn the basics to create a simple web application using angular and a simple backend api with asp.net in c#!</p>', N'[\"Angular\",\"Asp.Net\",\".Net\",\"C#\",\"Web Development\"]', 90, 45, 5);\r\nINSERT [dbo].[Lessons] ([id], [user], [titel], [lesson], [description], [tags], [duration], [price], [rating]) VALUES (21, 1, N'Plattdeutsch Lernen', 0, N'<p>Hier bring ich platt bei.</p>', N'[\"Platt\",\"German\"]', 35, 20, 4);\r\nSET IDENTITY_INSERT [dbo].[Lessons] OFF;\r\n\r\nSET IDENTITY_INSERT [dbo].[Users] ON;\r\n\r\nINSERT [dbo].[Users] ([id], [password], [name_display], [name_first], [name_last], [address_street], [address_city], [address_country], [address_postalCode], [mail]) VALUES (1, N'HAPM2E2ASE9KL80pQ5IPmU+R9puh/dv0vE5eQ2Ty58arSzq65CgLTc08FxNtmihS', N'Stulu', N'Julian', N'Bents', N'Musterstraﬂe, 25', N'Norden', N'Deutschland', N'26506', N'abc@gmail.com');\r\n\r\nSET IDENTITY_INSERT [dbo].[Users] OFF;\r\n\r\nINSERT [dbo].[UserStats] ([userID], [rating], [ratingCount], [description], [headline], [avatarURL]) VALUES (1, 4, 28, N'Hello, this is my profile. I''m new here!', N'New User!', N'https://avatars.githubusercontent.com/u/58867625?v=4');\r\n");
			}
		}

		ConfigureApp();
	}
	public void ConfigureApp()
	{

		app.UseDefaultFiles();
		app.UseStaticFiles();
		// Configure the HTTP request pipeline.
		app.UseAuthorization();
		app.MapControllers();
		app.MapFallbackToFile("/index.html");
	}

	public void Run()
	{
		// close the docker instances to open ports if running inside visual studio
		app.Run();
	}

	public string GetConnectionString()
	{
		string? ConnectionString = Configuration.GetConnectionString("DefaultConnection");
		if (ConnectionString != null) {
			string? environment = Environment.GetEnvironmentVariable(ConnectionString);
			if (environment != null)
				return environment;
		}

		string? Local = Configuration.GetConnectionString("LocalConnection");

		return Local ?? "";
	}

	public bool UseMySQL()
	{
		string? environment = Environment.GetEnvironmentVariable("USE_MYSQL");
		if(environment != null)
		{
			return environment == "1" || environment.ToLower() == "true";
		}
		return false;
	}
}