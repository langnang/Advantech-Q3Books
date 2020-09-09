using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Q3Books.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user = table.Column<string>(nullable: true),
                    book = table.Column<string>(nullable: true),
                    datetime = table.Column<DateTime>(nullable: false),
                    custom = table.Column<bool>(nullable: false),
                    annotation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    tips = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_config", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    team_code = table.Column<string>(nullable: true),
                    job_number = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    organization = table.Column<string>(nullable: true),
                    floor = table.Column<string>(nullable: true),
                    character = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "config");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
