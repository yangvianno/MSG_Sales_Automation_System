using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class themtypecrm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CRM_Products_Type_type_infoid_type",
                table: "CRM_Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Type",
                table: "Type");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "CRM_Types");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CRM_Types",
                table: "CRM_Types",
                column: "id_type");

            migrationBuilder.AddForeignKey(
                name: "FK_CRM_Products_CRM_Types_type_infoid_type",
                table: "CRM_Products",
                column: "type_infoid_type",
                principalTable: "CRM_Types",
                principalColumn: "id_type",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CRM_Products_CRM_Types_type_infoid_type",
                table: "CRM_Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CRM_Types",
                table: "CRM_Types");

            migrationBuilder.RenameTable(
                name: "CRM_Types",
                newName: "Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Type",
                table: "Type",
                column: "id_type");

            migrationBuilder.AddForeignKey(
                name: "FK_CRM_Products_Type_type_infoid_type",
                table: "CRM_Products",
                column: "type_infoid_type",
                principalTable: "Type",
                principalColumn: "id_type",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
