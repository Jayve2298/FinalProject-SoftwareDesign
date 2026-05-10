using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_Jenotan.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "PartsUsageReports");

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "PartsUsedForms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "PartsUsedForms");

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "PartsUsageReports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
