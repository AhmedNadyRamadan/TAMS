using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TASM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHomeV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tas_Labs_LabId",
                table: "Tas");

            migrationBuilder.DropIndex(
                name: "IX_Tas_LabId",
                table: "Tas");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Labs");

            migrationBuilder.AddColumn<int>(
                name: "LabId1",
                table: "Tas",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TaId",
                table: "Labs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tas_LabId1",
                table: "Tas",
                column: "LabId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tas_Labs_LabId1",
                table: "Tas",
                column: "LabId1",
                principalTable: "Labs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tas_Labs_LabId1",
                table: "Tas");

            migrationBuilder.DropIndex(
                name: "IX_Tas_LabId1",
                table: "Tas");

            migrationBuilder.DropColumn(
                name: "LabId1",
                table: "Tas");

            migrationBuilder.DropColumn(
                name: "TaId",
                table: "Labs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Labs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Tas_LabId",
                table: "Tas",
                column: "LabId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tas_Labs_LabId",
                table: "Tas",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "Id");
        }
    }
}
