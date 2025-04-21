using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TASM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabWithDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Attended",
                table: "SessionsStudents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Labs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attended",
                table: "SessionsStudents");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Labs");
        }
    }
}
