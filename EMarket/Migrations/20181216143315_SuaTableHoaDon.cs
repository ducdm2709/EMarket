using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Migrations
{
    public partial class SuaTableHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HoaDon");

            migrationBuilder.AlterColumn<int>(
                name: "SoLan",
                table: "TopSelling",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Hinh",
                table: "HangHoa",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLan",
                table: "TopSelling",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AddColumn<double>(
                name: "TongTien",
                table: "HoaDon",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Hinh",
                table: "HangHoa",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
