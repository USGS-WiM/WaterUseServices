using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WaterUseDB.Migrations
{
    public partial class uniquefieldsAndRegionFIPsCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UseTypes_Code",
                table: "UseTypes");

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

            migrationBuilder.AlterColumn<int>(
                name: "UseTypeID",
                table: "Sources",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FIPSCode",
                table: "Regions",
                type: "varchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "12");

            migrationBuilder.CreateIndex(
                name: "IX_UseTypes_Code",
                table: "UseTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_Abbreviation",
                table: "UnitTypes",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusTypes_Code",
                table: "StatusTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceTypes_Code",
                table: "SourceTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_FacilityCode",
                table: "Sources",
                column: "FacilityCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ShortName",
                table: "Regions",
                column: "ShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Username",
                table: "Managers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypes_Code",
                table: "CategoryTypes",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UseTypes_Code",
                table: "UseTypes");

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
                name: "IX_Sources_FacilityCode",
                table: "Sources");

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

            migrationBuilder.DropColumn(
                name: "FIPSCode",
                table: "Regions");

            migrationBuilder.AlterColumn<int>(
                name: "UseTypeID",
                table: "Sources",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.CreateIndex(
                name: "IX_UseTypes_Code",
                table: "UseTypes",
                column: "Code");

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
    }
}
