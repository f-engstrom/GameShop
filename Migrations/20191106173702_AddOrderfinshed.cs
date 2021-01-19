using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameShop.Migrations
{
    public partial class AddOrderfinshed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderFinishedDate",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderFinishedDate",
                table: "Order");
        }
    }
}
