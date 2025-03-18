using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class ThemChiTieuSoLuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            

            migrationBuilder.CreateTable(
                name: "ChiTieuSoLuongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    MaLoaiChiTieu = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTieuSoLuongs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTieuSoLuongs");

            migrationBuilder.DropColumn(
                name: "STT0",
                table: "CPAReportVMs");

            migrationBuilder.RenameColumn(
                name: "GroupTitle4",
                table: "CPAReportVMs",
                newName: "GroupTile4");

            migrationBuilder.RenameColumn(
                name: "GroupTitle3",
                table: "CPAReportVMs",
                newName: "GroupTile3");
        }
    }
}
