using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterUseDB.Migrations
{
    public partial class cleanupRecources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permits_StatusTypes_StatusTypeID",
                table: "Permits");

            migrationBuilder.DropForeignKey(
                name: "FK_Permits_UnitTypes_UnitTypeID",
                table: "Permits");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_CategoryTypes_CategoryTypeID",
                table: "Sources");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "UnitTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StatusTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "StatusTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryTypeID",
                table: "Sources",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "FacilityCode",
                table: "Sources",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<double>(
                name: "WellCapacity",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "UnitTypeID",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeID",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "PermitNO",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IntakeCapacity",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddForeignKey(
                name: "FK_Permits_StatusTypes_StatusTypeID",
                table: "Permits",
                column: "StatusTypeID",
                principalTable: "StatusTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permits_UnitTypes_UnitTypeID",
                table: "Permits",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_CategoryTypes_CategoryTypeID",
                table: "Sources",
                column: "CategoryTypeID",
                principalTable: "CategoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permits_StatusTypes_StatusTypeID",
                table: "Permits");

            migrationBuilder.DropForeignKey(
                name: "FK_Permits_UnitTypes_UnitTypeID",
                table: "Permits");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_CategoryTypes_CategoryTypeID",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "FacilityCode",
                table: "Sources");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "UnitTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StatusTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "StatusTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryTypeID",
                table: "Sources",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WellCapacity",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitTypeID",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeID",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PermitNO",
                table: "Permits",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<double>(
                name: "IntakeCapacity",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Permits",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permits_StatusTypes_StatusTypeID",
                table: "Permits",
                column: "StatusTypeID",
                principalTable: "StatusTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permits_UnitTypes_UnitTypeID",
                table: "Permits",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_CategoryTypes_CategoryTypeID",
                table: "Sources",
                column: "CategoryTypeID",
                principalTable: "CategoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
