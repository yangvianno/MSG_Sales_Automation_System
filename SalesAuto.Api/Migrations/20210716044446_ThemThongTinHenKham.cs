using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class ThemThongTinHenKham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STT = table.Column<int>(type: "int", nullable: false),
                    MaLichKham = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTaoLich = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayDatLichKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DienThoaiVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenChiNhanh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenhNhanKhamVMs",
                columns: table => new
                {
                    MaBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiDichVu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiPT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenLead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenBenhNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamSinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiamGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoanhThu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nguon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "BenhVienKhamVMs",
                columns: table => new
                {
                    MaBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiamGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoanhThu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "BenhVienVMs",
                columns: table => new
                {
                    MaBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
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
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "BenhNhanKhamVMs");

            migrationBuilder.DropTable(
                name: "BenhVienKhamVMs");

            migrationBuilder.DropTable(
                name: "BenhVienVMs");

            migrationBuilder.DropTable(
                name: "LeadVMs");
        }
    }
}
