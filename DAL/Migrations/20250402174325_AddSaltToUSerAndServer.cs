using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSaltToUSerAndServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");

            migrationBuilder.AddColumn<string>(
                name: "HashPassword",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Хэшированный админский пароль сервера");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Админский логин сервера");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Servers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HashPassword",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Servers");
        }
    }
}
