using LearnApp.Server.DBContext;
using Microsoft.EntityFrameworkCore;
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

		builder.Services.AddDbContext<UserContext>(options =>
			options.UseSqlServer(GetConnectionString()));

		app = builder.Build();
		
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<UserContext>();
			db.Database.Migrate();
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
}