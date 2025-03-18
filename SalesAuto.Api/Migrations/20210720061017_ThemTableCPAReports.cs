using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAuto.Api.Migrations
{
    public partial class ThemTableCPAReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CPAReportVMs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupTitle1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupTitle2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupTile3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupTile4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenhVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPAReportVMs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPAReportVMs");
        }
    }
}
