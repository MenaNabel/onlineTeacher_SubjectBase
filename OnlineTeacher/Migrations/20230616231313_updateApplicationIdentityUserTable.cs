using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTeacher.Migrations
{
    public partial class updateApplicationIdentityUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MacAddress2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitorIP",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitorIP2",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Student",
                newName: "school");

            migrationBuilder.RenameColumn(
                name: "VisitorIpsAssignedNo",
                table: "AspNetUsers",
                newName: "studentID");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "لا يوجد هاتف لهذا الشخص",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_studentID",
                table: "AspNetUsers",
                column: "studentID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Student_studentID",
                table: "AspNetUsers",
                column: "studentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Student_studentID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_studentID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "school",
                table: "Student",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "studentID",
                table: "AspNetUsers",
                newName: "VisitorIpsAssignedNo");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "لا يوجد هاتف لهذا الشخص");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}
