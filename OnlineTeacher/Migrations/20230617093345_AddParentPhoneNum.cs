using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTeacher.Migrations
{
    public partial class AddParentPhoneNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentPhone",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "لا يوجد هاتف لهذا الشخص");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentPhone",
                table: "Student");
        }
    }
}
