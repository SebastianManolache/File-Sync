using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiProject.Migrations
{
    public partial class Rename_Type_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "File",
                newName: "Type");

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "File",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "File",
                newName: "FileType");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "File",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
