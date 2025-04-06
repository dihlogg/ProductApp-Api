using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WavesOfFoodDemo.Server.Migrations
{
    /// <inheritdoc />
    public partial class updaterecommendcriteriaproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BateryCapacity",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectType",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpuType",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailsType",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RamType",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RomType",
                table: "ProductInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenSize",
                table: "ProductInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BateryCapacity",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "ConnectType",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "CpuType",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "DetailsType",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "RamType",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "RomType",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "ProductInfos");
        }
    }
}
