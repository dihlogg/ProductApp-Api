using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WavesOfFoodDemo.Server.Migrations
{
    /// <inheritdoc />
    public partial class addproductIdtotableproductinfohistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductInfoHistorys",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductInfoHistorys");
        }
    }
}
