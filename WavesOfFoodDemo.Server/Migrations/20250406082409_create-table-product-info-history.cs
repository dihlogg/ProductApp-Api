using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WavesOfFoodDemo.Server.Migrations
{
    /// <inheritdoc />
    public partial class createtableproductinfohistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductInfoHistoryId",
                table: "ProductImages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductInfoHistoryId",
                table: "CartDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductInfoHistorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CpuType = table.Column<string>(type: "text", nullable: true),
                    RamType = table.Column<string>(type: "text", nullable: true),
                    RomType = table.Column<string>(type: "text", nullable: true),
                    ScreenSize = table.Column<string>(type: "text", nullable: true),
                    BateryCapacity = table.Column<string>(type: "text", nullable: true),
                    DetailsType = table.Column<string>(type: "text", nullable: true),
                    ConnectType = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfoHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInfoHistorys_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductInfoHistoryId",
                table: "ProductImages",
                column: "ProductInfoHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ProductInfoHistoryId",
                table: "CartDetails",
                column: "ProductInfoHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfoHistorys_CategoryId",
                table: "ProductInfoHistorys",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_ProductInfoHistorys_ProductInfoHistoryId",
                table: "CartDetails",
                column: "ProductInfoHistoryId",
                principalTable: "ProductInfoHistorys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages",
                column: "ProductInfoHistoryId",
                principalTable: "ProductInfoHistorys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_ProductInfoHistorys_ProductInfoHistoryId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductInfoHistorys_ProductInfoHistoryId",
                table: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductInfoHistorys");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductInfoHistoryId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_ProductInfoHistoryId",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "ProductInfoHistoryId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductInfoHistoryId",
                table: "CartDetails");
        }
    }
}
