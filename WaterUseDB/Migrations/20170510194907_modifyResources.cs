using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace WaterUseDB.Migrations
{
    public partial class modifyResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<PostgisPoint>(
                name: "Location",
                table: "Sources",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermitNO",
                table: "Permits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "PermitNO",
                table: "Permits");
        }
    }
}
