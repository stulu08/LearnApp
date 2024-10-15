﻿using Microsoft.EntityFrameworkCore;

namespace LearnApp.Server.DBContext
{
	public class UserContext : DbContext {
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{

		}
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>();
		}
	}
}
