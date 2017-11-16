using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WaterUseDB.Migrations
{
    public partial class addUseType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
            //    table: "CatagoryCoefficients");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Managers_Roles_RoleID",
            //    table: "Managers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Sources_Regions_RegionID",
            //    table: "Sources");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Sources_SourceTypes_SourceTypeID",
            //    table: "Sources");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TimeSeries_UnitTypes_UnitTypeID",
            //    table: "TimeSeries");

            ////migrationBuilder.AddColumn<int>(
            ////    name: "UseTypeID",
            ////    table: "Sources",
            ////    type: "int4",
            ////    nullable: false,
            ////    defaultValue: 0);


            //migrationBuilder.CreateTable(
            //    name: "UseTypes",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int4", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //        Code = table.Column<string>(type: "text", nullable: false),
            //        Description = table.Column<string>(type: "text", nullable: true),
            //        Name = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UseTypes", x => x.ID);
            //    });

            //migrationBuilder.Sql("INSERT INTO 'UseTypes' ('Name', 'Code', 'Description') VALUES ('Temp', 'temp', 'temp')");
            //// Default value for FK points to department created above, with
            //// defaultValue changed to 1 in following AddColumn statement.

            //migrationBuilder.AddColumn<int>(
            //    name: "UseTypeID",
            //    table: "Sources",
            //    type: "int4",
            //    nullable: false,
            //    defaultValue: 1);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Sources_UseTypeID",
            //    table: "Sources",
            //    column: "UseTypeID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UseTypes_Code",
            //    table: "UseTypes",
            //    column: "Code");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CatagoryCoefficients_CatagoryTypes_CatagoryTypeID",
            //    table: "CatagoryCoefficients",
            //    column: "CatagoryTypeID",
            //    principalTable: "CatagoryTypes",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Managers_Roles_RoleID",
            //    table: "Managers",
            //    column: "RoleID",
            //    principalTable: "Roles",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sources_Regions_RegionID",
            //    table: "Sources",
            //    column: "RegionID",
            //    principalTable: "Regions",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sources_SourceTypes_SourceTypeID",
            //    table: "Sources",
            //    column: "SourceTypeID",
            //    principalTable: "SourceTypes",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sources_UseTypes_UseTypeID",
            //    table: "Sources",
            //    column: "UseTypeID",
            //    principalTable: "UseTypes",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TimeSeries_UnitTypes_UnitTypeID",
            //    table: "TimeSeries",
            //    column: "UnitTypeID",
            //    principalTable: "UnitTypes",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);
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
                name: "FK_Sources_UseTypes_UseTypeID",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                table: "TimeSeries");

            migrationBuilder.DropTable(
                name: "UseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Sources_UseTypeID",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "UseTypeID",
                table: "Sources");

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
    }
}
