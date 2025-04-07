using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WavesOfFoodDemo.Server.Migrations
{
    /// <inheritdoc />
    public partial class addproducthistorytoproductimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages",
                column: "ProductInfoHistoryId",
                principalTable: "ProductInfoHistorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages",
                column: "ProductInfoHistoryId",
                principalTable: "ProductInfoHistorys",
                principalColumn: "Id");
        }
    }
}
