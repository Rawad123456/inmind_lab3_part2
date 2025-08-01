using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrollmentStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Enrollments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Enrollments");
        }
    }
}
