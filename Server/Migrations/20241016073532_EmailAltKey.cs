using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class EmailAltKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_id",
                table: "Users",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_id",
                table: "Users");
        }
    }
}
