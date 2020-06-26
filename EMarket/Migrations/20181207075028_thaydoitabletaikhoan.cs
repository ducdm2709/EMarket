using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class thaydoitabletaikhoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_ThongTinTaiKhoan",
                table: "TaiKhoan");

            migrationBuilder.DropIndex(
                name: "IX_TaiKhoan_ThongTinTaiKhoanID",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "ThongTinTaiKhoanID",
                table: "TaiKhoan");

            migrationBuilder.AlterColumn<string>(
                name: "SDT",
                table: "ThongTinTaiKhoan",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "ThongTinTaiKhoan",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "ThongTinTaiKhoan",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoanId",
                table: "ThongTinTaiKhoan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan",
                column: "TaiKhoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongTinTaiKhoan_TaiKhoan",
                table: "ThongTinTaiKhoan",
                column: "TaiKhoanId",
                principalTable: "TaiKhoan",
                principalColumn: "TaiKhoanID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinTaiKhoan_TaiKhoan",
                table: "ThongTinTaiKhoan");

            migrationBuilder.DropIndex(
                name: "IX_ThongTinTaiKhoan_TaiKhoanId",
                table: "ThongTinTaiKhoan");

            migrationBuilder.DropColumn(
                name: "TaiKhoanId",
                table: "ThongTinTaiKhoan");

            migrationBuilder.AlterColumn<string>(
                name: "SDT",
                table: "ThongTinTaiKhoan",
                unicode: false,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "ThongTinTaiKhoan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "ThongTinTaiKhoan",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "ThongTinTaiKhoanID",
                table: "TaiKhoan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_ThongTinTaiKhoanID",
                table: "TaiKhoan",
                column: "ThongTinTaiKhoanID");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_ThongTinTaiKhoan",
                table: "TaiKhoan",
                column: "ThongTinTaiKhoanID",
                principalTable: "ThongTinTaiKhoan",
                principalColumn: "ThongTinTaiKhoanID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
