using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class ThemEmailVaoTaiKhoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TaiKhoan",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "TaiKhoan");
        }
    }
}
