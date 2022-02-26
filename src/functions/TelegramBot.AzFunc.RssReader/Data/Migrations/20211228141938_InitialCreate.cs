using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.AzFunc.RssReader.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelItems",
                columns: table => new
                {
                    ItemLink = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelItems", x => x.ItemLink);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelItems");
        }
    }
}
