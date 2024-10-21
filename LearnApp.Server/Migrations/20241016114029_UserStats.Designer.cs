﻿// <auto-generated />
using LearnApp.Server.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearnApp.Server.Migrations
{
    [DbContext(typeof(DBContext.UserContext))]
    [Migration("20241016114029_UserStats")]
    partial class UserStats
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LearnApp.Server.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("id")
                        .IsUnique();

                    b.HasIndex("mail")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LearnApp.Server.UserStats", b =>
                {
                    b.Property<int>("userID")
                        .HasColumnType("int");

                    b.Property<string>("avatarURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("headline")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.Property<int>("ratingCount")
                        .HasColumnType("int");

                    b.HasKey("userID");

                    b.ToTable("UserStats");
                });

            modelBuilder.Entity("LearnApp.Server.User", b =>
                {
                    b.OwnsOne("LearnApp.Server.Address", "address", b1 =>
                        {
                            b1.Property<int>("Userid")
                                .HasColumnType("int");

                            b1.Property<string>("city")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("postalCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Userid");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("Userid");
                        });

                    b.OwnsOne("LearnApp.Server.Name", "name", b1 =>
                        {
                            b1.Property<int>("Userid")
                                .HasColumnType("int");

                            b1.Property<string>("display")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("first")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("last")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Userid");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("Userid");
                        });

                    b.Navigation("address")
                        .IsRequired();

                    b.Navigation("name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
