using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTeacher.Migrations
{
    public partial class macadress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacAddress2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MacAddress2",
                table: "AspNetUsers");
        }
    }
}
