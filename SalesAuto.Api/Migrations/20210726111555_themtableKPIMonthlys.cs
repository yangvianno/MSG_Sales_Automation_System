using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class themtableKPIMonthlys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenhVienKham",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "BenhVienPhauThuat",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "NgayKham",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "NgayPhauThuat",
                table: "Leads");

            migrationBuilder.CreateTable(
                name: "KPIMonthlys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    ActualDigitalCost = table.Column<long>(type: "bigint", nullable: false),
                    ActualBranding = table.Column<long>(type: "bigint", nullable: false),
                    BudgetDigitalCost = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIMonthlys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadVMs",
                columns: table => new
                {
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BenhVienKham = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayPhauThuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BenhVienPhauThuat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STT = table.Column<int>(type: "int", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoPhu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nguon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhThanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayImport = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPIMonthlys");

            migrationBuilder.DropTable(
                name: "LeadVMs");

            migrationBuilder.AddColumn<string>(
                name: "BenhVienKham",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenhVienPhauThuat",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKham",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayPhauThuat",
                table: "Leads",
                type: "datetime2",
                nullable: true);
        }
    }
}
