using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Id Сервера.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Ссылка на сервер к которому привязан пользоваетль.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Id Пользователя.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Имя пользователя полностью. (ФИО)."),
                    HashPassword = table.Column<string>(type: "text", nullable: true, comment: "Хэш пароля пользователя."),
                    VpnLinks = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Vless ссылки на vpn."),
                    ServerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Ссылка на сервер к которому привязан пользоваетль."),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Email пользователя.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
