using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrollmentStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToStudentAndCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Students",
                schema: "public",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "Enrollments",
                schema: "public",
                newName: "Enrollments");

            migrationBuilder.RenameTable(
                name: "Courses",
                schema: "public",
                newName: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Courses");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Students",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Enrollments",
                newName: "Enrollments",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Courses",
                newSchema: "public");
        }
    }
}
