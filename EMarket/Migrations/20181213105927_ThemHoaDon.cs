using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class ThemHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    HoaDonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NgayLapHoaDon = table.Column<DateTime>(nullable: false),
                    TinhTrang = table.Column<bool>(nullable: false),
                    TongTien = table.Column<double>(nullable: false),
                    TenKhachHang = table.Column<string>(nullable: false),
                    DiaChi = table.Column<string>(nullable: false),
                    SDT = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.HoaDonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDon");
        }
    }
}
