using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterUseDB.Migrations
{
    public partial class Restrictdeletebehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Roles_RoleID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Regions_RegionID",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeID",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                table: "TimeSeries");

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients",
                column: "CatagoryTypeID",
                principalTable: "CatagoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Roles_RoleID",
                table: "Managers",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Regions_RegionID",
                table: "Sources",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeID",
                table: "Sources",
                column: "SourceTypeID",
                principalTable: "SourceTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                table: "TimeSeries",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Roles_RoleID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Regions_RegionID",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeID",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                table: "TimeSeries");

            migrationBuilder.AddForeignKey(
                name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
                table: "CatagoryCoefficients",
                column: "CatagoryTypeID",
                principalTable: "CatagoryTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Roles_RoleID",
                table: "Managers",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Regions_RegionID",
                table: "Sources",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeID",
                table: "Sources",
                column: "SourceTypeID",
                principalTable: "SourceTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                table: "TimeSeries",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
