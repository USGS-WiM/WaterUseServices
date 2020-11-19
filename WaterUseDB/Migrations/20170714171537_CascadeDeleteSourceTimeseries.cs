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
                name: "FK_CatagoryCoefficient_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficient");

            migrationBuilder.DropForeignKey(
                name: "FK_CatagoryCoefficient_Regions_RegionID",
                table: "CatagoryCoefficient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatagoryCoefficient",
                table: "CatagoryCoefficient");

            migrationBuilder.RenameTable(
                name: "CatagoryCoefficient",
                newName: "CatagoryCoefficients");

            migrationBuilder.RenameIndex(
                name: "IX_CatagoryCoefficient_RegionID",
                table: "CatagoryCoefficients",
                newName: "IX_CatagoryCoefficients_RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_CatagoryCoefficient_CatagoryTypeID",
                table: "CatagoryCoefficients",
                newName: "IX_CatagoryCoefficients_CatagoryTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatagoryCoefficients",
                table: "CatagoryCoefficients",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients",
                column: "CatagoryTypeID",
                principalTable: "CatagoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficients_Regions_RegionID",
                table: "CatagoryCoefficients",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients");

            migrationBuilder.DropForeignKey(
                name: "FK_CatagoryCoefficients_Regions_RegionID",
                table: "CatagoryCoefficients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatagoryCoefficients",
                table: "CatagoryCoefficients");

            migrationBuilder.RenameTable(
                name: "CatagoryCoefficients",
                newName: "CatagoryCoefficient");

            migrationBuilder.RenameIndex(
                name: "IX_CatagoryCoefficients_RegionID",
                table: "CatagoryCoefficient",
                newName: "IX_CatagoryCoefficient_RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_CatagoryCoefficients_CatagoryTypeID",
                table: "CatagoryCoefficient",
                newName: "IX_CatagoryCoefficient_CatagoryTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatagoryCoefficient",
                table: "CatagoryCoefficient",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficient_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficient",
                column: "CatagoryTypeID",
                principalTable: "CatagoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficient_Regions_RegionID",
                table: "CatagoryCoefficient",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
