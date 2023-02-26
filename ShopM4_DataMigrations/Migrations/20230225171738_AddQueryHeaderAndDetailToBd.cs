using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopM4_DataMigrations
{
    public partial class AddQueryHeaderAndDetailToBd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueryHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QueryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryHeader_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueryDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueryDetail_QueryHeader_QueryHeaderId",
                        column: x => x.QueryHeaderId,
                        principalTable: "QueryHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueryDetail_ProductId",
                table: "QueryDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryDetail_QueryHeaderId",
                table: "QueryDetail",
                column: "QueryHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryHeader_ApplicationUserId",
                table: "QueryHeader",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueryDetail");

            migrationBuilder.DropTable(
                name: "QueryHeader");
        }
    }
}
