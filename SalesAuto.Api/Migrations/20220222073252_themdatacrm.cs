using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class themdatacrm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_Categories",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "int", nullable: false)
                        ,
                    category_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Categories", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "CRM_Districts",
                columns: table => new
                {
                    id_district = table.Column<int>(type: "int", nullable: false)
                        ,
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_parent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Districts", x => x.id_district);
                });

            migrationBuilder.CreateTable(
                name: "CRM_Provinces",
                columns: table => new
                {
                    id_province = table.Column<int>(type: "int", nullable: false)
                       ,
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Provinces", x => x.id_province);
                });

            migrationBuilder.CreateTable(
                name: "CRM_Stores",
                columns: table => new
                {
                    id_store = table.Column<int>(type: "int", nullable: false)
                        ,
                    store_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Stores", x => x.id_store);
                });

            migrationBuilder.CreateTable(
                name: "CRM_Wards",
                columns: table => new
                {
                    id_ward = table.Column<int>(type: "int", nullable: false)
                        ,
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_parent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Wards", x => x.id_ward);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    id_type = table.Column<int>(type: "int", nullable: false)
                        ,
                    type_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.id_type);
                });

            migrationBuilder.CreateTable(
                name: "CRM_Products",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "int", nullable: false)
                        ,
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    begin_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_infoid_category = table.Column<int>(type: "int", nullable: true),
                    type_infoid_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_Products", x => x.id_product);
                    table.ForeignKey(
                        name: "FK_CRM_Products_CRM_Categories_category_infoid_category",
                        column: x => x.category_infoid_category,
                        principalTable: "CRM_Categories",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CRM_Products_Type_type_infoid_type",
                        column: x => x.type_infoid_type,
                        principalTable: "Type",
                        principalColumn: "id_type",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CRM_Products_category_infoid_category",
                table: "CRM_Products",
                column: "category_infoid_category");

            migrationBuilder.CreateIndex(
                name: "IX_CRM_Products_type_infoid_type",
                table: "CRM_Products",
                column: "type_infoid_type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_Districts");

            migrationBuilder.DropTable(
                name: "CRM_Products");

            migrationBuilder.DropTable(
                name: "CRM_Provinces");

            migrationBuilder.DropTable(
                name: "CRM_Stores");

            migrationBuilder.DropTable(
                name: "CRM_Wards");

            migrationBuilder.DropTable(
                name: "CRM_Categories");

            migrationBuilder.DropTable(
                name: "Type");
        }
    }
}
