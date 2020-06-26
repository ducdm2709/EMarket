using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class CreateKhoHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhoHang",
                columns: table => new
                {
                    KhoHangID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SoLuong = table.Column<int>(nullable: false),
                    HangHoaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoHang", x => x.KhoHangID);
                    table.ForeignKey(
                        name: "FK_KhoHang_HangHoa_HangHoaID",
                        column: x => x.HangHoaID,
                        principalTable: "HangHoa",
                        principalColumn: "HangHoaID",
                        onDelete: ReferentialAction.Cascade);
                });

         
            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_LoaiID",
                table: "HangHoa",
                column: "LoaiID");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_NhaCungCapID",
                table: "HangHoa",
                column: "NhaCungCapID");

            migrationBuilder.CreateIndex(
                name: "IX_KhoHang_HangHoaID",
                table: "KhoHang",
                column: "HangHoaID");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_ThongTinTaiKhoanID",
                table: "TaiKhoan",
                column: "ThongTinTaiKhoanID");

            migrationBuilder.CreateIndex(
                name: "IX_TopSelling_HangHoaID",
                table: "TopSelling",
                column: "HangHoaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KhoHang");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "TopSelling");

            migrationBuilder.DropTable(
                name: "ThongTinTaiKhoan");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropTable(
                name: "Loai");

            migrationBuilder.DropTable(
                name: "NhaCungCap");
        }
    }
}
