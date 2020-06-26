using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class ThemChiTietHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    ChiTietHoaDonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HoaDonId = table.Column<int>(nullable: false),
                    HangHoaId = table.Column<int>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    TongTien = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => x.ChiTietHoaDonId);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HangHoa_HangHoaId",
                        column: x => x.HangHoaId,
                        principalTable: "HangHoa",
                        principalColumn: "HangHoaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HoaDon_HoaDonId",
                        column: x => x.HoaDonId,
                        principalTable: "HoaDon",
                        principalColumn: "HoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_HangHoaId",
                table: "ChiTietHoaDon",
                column: "HangHoaId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_HoaDonId",
                table: "ChiTietHoaDon",
                column: "HoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");
        }
    }
}
