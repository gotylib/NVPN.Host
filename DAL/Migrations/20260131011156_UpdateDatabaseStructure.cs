using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VlessProfiles");

            migrationBuilder.DropColumn(
                name: "ServerName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VpnLinks",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "ServerName",
                table: "Servers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Имя пользователя полностью. (ФИО).");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.");

            migrationBuilder.AlterColumn<string>(
                name: "HashPassword",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Хэш пароля пользователя.");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Email пользователя.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Id Пользователя.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Id Сервера.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "AdminLogin",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminPassword",
                table: "Servers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Servers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserServers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ServerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServers", x => new { x.UserId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_UserServers_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VlessLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ServerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VlessLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VlessLinks_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VlessLinks_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_Host",
                table: "Servers",
                column: "Host",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserServers_ServerId",
                table: "UserServers",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_ExpiryDate",
                table: "VlessLinks",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_IsActive",
                table: "VlessLinks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_ServerId",
                table: "VlessLinks",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_ServerId_IsActive",
                table: "VlessLinks",
                columns: new[] { "ServerId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_UserId",
                table: "VlessLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VlessLinks_UserId_IsActive",
                table: "VlessLinks",
                columns: new[] { "UserId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserServers");

            migrationBuilder.DropTable(
                name: "VlessLinks");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Servers_Host",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "AdminLogin",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "AdminPassword",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "Servers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                comment: "Имя пользователя полностью. (ФИО).",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "HashPassword",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "Хэш пароля пользователя.",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Email пользователя.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                comment: "Id Пользователя.",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "ServerName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Ссылка на сервер к которому привязан пользоваетль.");

            migrationBuilder.AddColumn<string>(
                name: "VpnLinks",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Vless ссылки на vpn.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Servers",
                type: "integer",
                nullable: false,
                comment: "Id Сервера.",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Админский логин сервера");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Servers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Хэшированный админский пароль сервера");

            migrationBuilder.AddColumn<string>(
                name: "ServerName",
                table: "Servers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Ссылка на сервер к которому привязан пользоваетль.");

            migrationBuilder.CreateTable(
                name: "VlessProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Flow = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Network = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    PublicKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Security = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sni = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Spx = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserInfo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VlessProfiles", x => x.Id);
                });
        }
    }
}
