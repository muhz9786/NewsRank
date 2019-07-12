using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsRank.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "newsdb");

            migrationBuilder.CreateTable(
                name: "tbl_news",
                schema: "newsdb",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false),
                    news_rank = table.Column<int>(type: "int(11)", nullable: false),
                    news_title = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    news_url = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    submit_time = table.Column<DateTime>(nullable: true),
                    news_type = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    NEWS_CONTENT = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_news", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_news",
                schema: "newsdb");
        }
    }
}
