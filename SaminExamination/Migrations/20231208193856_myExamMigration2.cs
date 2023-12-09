using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaminExamination.Migrations
{
    public partial class myExamMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "invoiceSells",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "invoiceSells");
        }
    }
}
