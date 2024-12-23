using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class LessonsPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Lessons");
        }
    }
}
