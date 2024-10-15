class Program
{
	public static Startup startup;

	static void Main(string[] args) {
		startup = new Startup(args);
		startup.Run();
	}
}

public class Startup
{
	private WebApplicationBuilder builder;
	private WebApplication app;

	public Startup(String[] argv)
	{
		builder = WebApplication.CreateBuilder(argv);
		builder.Services.AddControllers();

		// Add services to the container.
		app = builder.Build();
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
}