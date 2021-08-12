using Microsoft.EntityFrameworkCore.Migrations;

namespace RuS.Infrastructure.Migrations
{
    public partial class PriorityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Priorities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Priorities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Priorities");

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Priorities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
