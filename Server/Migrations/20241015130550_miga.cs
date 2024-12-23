using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Server.Migrations
{
	/// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
	public partial class miga : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
	{
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_display = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_first = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_last = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_postalCode = table.Column<int>(type: "int", nullable: false),
                    mail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

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
                name: "Users");
        }
    }
}
