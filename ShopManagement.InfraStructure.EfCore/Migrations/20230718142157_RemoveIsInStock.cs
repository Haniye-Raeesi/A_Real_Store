using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManagement.InfraStructure.EfCore.Migrations
{
    public partial class RemoveIsInStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
