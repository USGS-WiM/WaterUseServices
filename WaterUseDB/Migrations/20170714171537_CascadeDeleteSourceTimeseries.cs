using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterUseDB.Migrations
{
    public partial class CascadeDeleteSourceTimeseries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCoefficient_CategoryTypes_CategoryTypeID",
                table: "CategoryCoefficient");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCoefficient_Regions_RegionID",
                table: "CategoryCoefficient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryCoefficient",
                table: "CategoryCoefficient");

            migrationBuilder.RenameTable(
                name: "CategoryCoefficient",
                newName: "CategoryCoefficients");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCoefficient_RegionID",
                table: "CategoryCoefficients",
                newName: "IX_CategoryCoefficients_RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCoefficient_CategoryTypeID",
                table: "CategoryCoefficients",
                newName: "IX_CategoryCoefficients_CategoryTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryCoefficients",
                table: "CategoryCoefficients",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCoefficients_CategoryTypes_CategoryTypeID",
                table: "CategoryCoefficients",
                column: "CategoryTypeID",
                principalTable: "CategoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCoefficients_Regions_RegionID",
                table: "CategoryCoefficients",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCoefficients_CategoryTypes_CategoryTypeID",
                table: "CategoryCoefficients");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCoefficients_Regions_RegionID",
                table: "CategoryCoefficients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryCoefficients",
                table: "CategoryCoefficients");

            migrationBuilder.RenameTable(
                name: "CategoryCoefficients",
                newName: "CategoryCoefficient");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCoefficients_RegionID",
                table: "CategoryCoefficient",
                newName: "IX_CategoryCoefficient_RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCoefficients_CategoryTypeID",
                table: "CategoryCoefficient",
                newName: "IX_CategoryCoefficient_CategoryTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryCoefficient",
                table: "CategoryCoefficient",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCoefficient_CategoryTypes_CategoryTypeID",
                table: "CategoryCoefficient",
                column: "CategoryTypeID",
                principalTable: "CategoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCoefficient_Regions_RegionID",
                table: "CategoryCoefficient",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
