using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resto.Migrations
{
    /// <inheritdoc />
    public partial class FixIncorrectDbRelations_OrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderedItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderedItemId",
                table: "OrderItems",
                column: "OrderedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrderDetailId",
                table: "Items",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_OrderItems_OrderDetailId",
                table: "Items",
                column: "OrderDetailId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_OrderItems_OrderDetailId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderedItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Items_OrderDetailId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderedItemId",
                table: "OrderItems",
                column: "OrderedItemId",
                unique: true);
        }
    }
}
