using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class themtableRemoveLeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "RemovedLeads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemovedLeads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemovedLeads");

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
                name: "LeadVMs",
                columns: table => new
                {
                    BenhVienKham = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenhVienPhauThuat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayImport = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayPhauThuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nguon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STT = table.Column<int>(type: "int", nullable: false),
                    SoPhu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhThanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
