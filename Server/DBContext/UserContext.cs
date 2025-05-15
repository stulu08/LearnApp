using Microsoft.EntityFrameworkCore;

namespace LearnApp.Server.DBContext
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
		public DbSet<UserStats> UserStats { get; set; }
		public DbSet<Lesson> Lessons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(u => u.id);
				entity.HasIndex(u => u.id).IsUnique();
				entity.HasIndex(u => u.mail).IsUnique();
			});

			modelBuilder.Entity<UserStats>(entity =>
			{
				entity.HasKey(u => u.userID);
			});

			modelBuilder.Entity<Lesson>(entity =>
			{
				entity.HasKey(l => l.id);
				entity.HasIndex(l => l.id).IsUnique();
			});

			// Set default max length for all string columns if not specified
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				foreach (var property in entity.GetProperties())
				{
					if (property.ClrType == typeof(string) &&
						property.GetMaxLength() == null &&
						!property.IsPrimaryKey())
					{
						property.SetMaxLength(255);
					}
				}
			}

			if (Database.IsMySql())
			{
				modelBuilder.Entity<Lesson>().Property(e => e.description).HasColumnType("LONGTEXT");
				modelBuilder.Entity<Lesson>().Property(e => e.tags).HasColumnType("LONGTEXT");
			}
			else
			{
				modelBuilder.Entity<Lesson>().Property(e => e.description).HasMaxLength(-1);
				modelBuilder.Entity<Lesson>().Property(e => e.tags).HasMaxLength(100);
			}

			base.OnModelCreating(modelBuilder);
		}

		public User? GetUser(int id)
		{
			User? user = Users.Find(id);
			if (user == null) return null;

			return new User()
			{
				address = user.address,
				name = user.name,
				id = user.id,
				mail = user.mail,
				password = user.password,
			};
		}
		public User MakeUserPublic(User user)
		{
			user.password = "";
			user.address.street = "";
			return user;
		}
		public User? GetUserTracked(int id)
		{
			return Users.Find(id);
		}
		public UserStats GetUserStats(int id)
		{
			UserStats? stats = UserStats.Find(id);
			if (stats != null) {
				return stats;
			}

			var entry = UserStats.Add(new UserStats
			{
				avatarURL = "https://demos.creative-tim.com/argon-dashboard-angular/assets/img/theme/team-4-800x800.jpg",
				rating = 0,
				ratingCount = 0,
				description = "Hello, this is my profile. I'm new here!",
				headline = "New User!",
				userID = id,
			});
			SaveChangesAsync();
			return entry.Entity;
		}
		public bool CheckUser(User user, string password)
		{
			bool status = Utils.PasswordVerify(user.password, password);
			return status;
		}

		public Lesson? GetLesson(int id)	{
			return Lessons.Find(id);
		}
	}
}
