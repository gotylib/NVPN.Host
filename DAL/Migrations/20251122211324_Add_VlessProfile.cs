using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_VlessProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VlessProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    UserInfo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Security = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Network = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Flow = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sni = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PublicKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Spx = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VlessProfiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VlessProfiles");
        }
    }
}
