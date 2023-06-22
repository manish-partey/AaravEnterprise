using Microsoft.EntityFrameworkCore.Migrations;

namespace AaravEnterprise.Migrations
{
    public partial class OrderDetailsPK2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
