using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class AdjustThongTinHoaDon2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan",
                column: "TaiKhoanId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan",
                column: "TaiKhoanId");
        }
    }
}
