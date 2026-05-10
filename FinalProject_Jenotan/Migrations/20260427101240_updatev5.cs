using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_Jenotan.Migrations
{
    /// <inheritdoc />
    public partial class updatev5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Inventories_InvenotryId",
                table: "Parts");

            migrationBuilder.RenameColumn(
                name: "InvenotryId",
                table: "Parts",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Parts_InvenotryId",
                table: "Parts",
                newName: "IX_Parts_InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Inventories_InventoryId",
                table: "Parts",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Inventories_InventoryId",
                table: "Parts");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Parts",
                newName: "InvenotryId");

            migrationBuilder.RenameIndex(
                name: "IX_Parts_InventoryId",
                table: "Parts",
                newName: "IX_Parts_InvenotryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Inventories_InvenotryId",
                table: "Parts",
                column: "InvenotryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
