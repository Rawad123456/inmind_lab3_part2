using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrollmentStructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
