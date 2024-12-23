using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class UserStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    ratingCount = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    headline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avatarURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.userID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStats");
        }
    }
}
