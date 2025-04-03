using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldComment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Servers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldComment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Servers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");
        }
    }
}
