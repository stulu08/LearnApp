using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string longText = "nvarchar(max)";
            if (migrationBuilder.IsMySql())
            {
                longText = "longtext";
            }


			migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user = table.Column<int>(type: "int", nullable: false),
                    titel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    lesson = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: longText, maxLength: 12000, nullable: false),
                    tags = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_display = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    name_first = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    name_last = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address_street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address_city = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address_country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address_postalCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    mail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    ratingCount = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    headline = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    avatarURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.userID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_id",
                table: "Lessons",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_id",
                table: "Users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_mail",
                table: "Users",
                column: "mail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserStats");
        }
    }
}
