using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopM4_DataMigrations
{
    public partial class AddMyModelToProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyModelId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_MyModelId",
                table: "Product",
                column: "MyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_MyModel_MyModelId",
                table: "Product",
                column: "MyModelId",
                principalTable: "MyModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_MyModel_MyModelId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_MyModelId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MyModelId",
                table: "Product");
        }
    }
}
