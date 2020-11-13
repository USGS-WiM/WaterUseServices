using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterUseDB.Migrations
{
    public partial class modHasIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UnitTypes_Abbreviation",
                table: "UnitTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_StatusTypes_Code",
                table: "StatusTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SourceTypes_Code",
                table: "SourceTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Roles_Name",
                table: "Roles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Regions_ShortName",
                table: "Regions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Managers_Username",
                table: "Managers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CategoryTypes_Code",
                table: "CategoryTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "UnitTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "StatusTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_Abbreviation",
                table: "UnitTypes",
                column: "Abbreviation");

            migrationBuilder.CreateIndex(
                name: "IX_StatusTypes_Code",
                table: "StatusTypes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SourceTypes_Code",
                table: "SourceTypes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ShortName",
                table: "Regions",
                column: "ShortName");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Username",
                table: "Managers",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypes_Code",
                table: "CategoryTypes",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UnitTypes_Abbreviation",
                table: "UnitTypes");

            migrationBuilder.DropIndex(
                name: "IX_StatusTypes_Code",
                table: "StatusTypes");

            migrationBuilder.DropIndex(
                name: "IX_SourceTypes_Code",
                table: "SourceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ShortName",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Managers_Username",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTypes_Code",
                table: "CategoryTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "UnitTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "StatusTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UnitTypes_Abbreviation",
                table: "UnitTypes",
                column: "Abbreviation");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_StatusTypes_Code",
                table: "StatusTypes",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SourceTypes_Code",
                table: "SourceTypes",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Roles_Name",
                table: "Roles",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Regions_ShortName",
                table: "Regions",
                column: "ShortName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Managers_Username",
                table: "Managers",
                column: "Username");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CategoryTypes_Code",
                table: "CategoryTypes",
                column: "Code");
        }
    }
}
