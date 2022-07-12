using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTeacher.Migrations
{
    public partial class ipv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitorIP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorIP2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorIpsAssignedNo",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorIP",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitorIP2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitorIpsAssignedNo",
                table: "AspNetUsers");
        }
    }
}
