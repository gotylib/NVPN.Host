using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Hash_Password_In_Server_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Servers");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Servers",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Servers",
                newName: "HashPassword");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Servers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");
        }
    }
}
