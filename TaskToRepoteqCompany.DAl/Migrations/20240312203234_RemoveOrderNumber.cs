using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskToRepoteqCompany.DAL.Migrations
{
    public partial class RemoveOrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderNumber",
                table: "Orders",
                type: "bigint",
                precision: 20,
                nullable: false,
                defaultValue: 0L);
        }
    }
}
