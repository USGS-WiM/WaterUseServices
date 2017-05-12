using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WaterUseDB.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatagoryTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoryTypes", x => x.ID);
                    table.UniqueConstraint("AK_CatagoryTypes_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ShortName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                    table.UniqueConstraint("AK_Regions_ShortName", x => x.ShortName);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                    table.UniqueConstraint("AK_Roles_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "SourceTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceTypes", x => x.ID);
                    table.UniqueConstraint("AK_SourceTypes_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "StatusTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTypes", x => x.ID);
                    table.UniqueConstraint("AK_StatusTypes_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "UnitTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Abbreviation = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypes", x => x.ID);
                    table.UniqueConstraint("AK_UnitTypes_Abbreviation", x => x.Abbreviation);
                });

            migrationBuilder.CreateTable(
                name: "CatagoryCoefficient",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CatagoryTypeID = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    RegionID = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoryCoefficient", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CatagoryCoefficient_CatagoryTypes_CatagoryTypeID",
                        column: x => x.CatagoryTypeID,
                        principalTable: "CatagoryTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatagoryCoefficient_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    OtherInfo = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    PrimaryPhone = table.Column<string>(nullable: true),
                    RoleID = table.Column<int>(nullable: false),
                    SecondaryPhone = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ID);
                    table.UniqueConstraint("AK_Managers_Username", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Managers_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CatagoryTypeID = table.Column<int>(nullable: false),
                    FacilityName = table.Column<string>(nullable: false),
                    RegionID = table.Column<int>(nullable: false),
                    SourceTypeID = table.Column<int>(nullable: false),
                    StationID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sources_CatagoryTypes_CatagoryTypeID",
                        column: x => x.CatagoryTypeID,
                        principalTable: "CatagoryTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sources_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sources_SourceTypes_SourceTypeID",
                        column: x => x.SourceTypeID,
                        principalTable: "SourceTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionManager",
                columns: table => new
                {
                    ManagerID = table.Column<int>(nullable: false),
                    RegionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionManager", x => new { x.ManagerID, x.RegionID });
                    table.ForeignKey(
                        name: "FK_RegionManager_Managers_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Managers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionManager_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permits",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IntakeCapacity = table.Column<double>(nullable: false),
                    SourceID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    StatusTypeID = table.Column<int>(nullable: false),
                    UnitTypeID = table.Column<int>(nullable: false),
                    WellCapacity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Permits_Sources_SourceID",
                        column: x => x.SourceID,
                        principalTable: "Sources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permits_StatusTypes_StatusTypeID",
                        column: x => x.StatusTypeID,
                        principalTable: "StatusTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permits_UnitTypes_UnitTypeID",
                        column: x => x.UnitTypeID,
                        principalTable: "UnitTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSeries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    SourceID = table.Column<int>(nullable: false),
                    UnitTypeID = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSeries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TimeSeries_Sources_SourceID",
                        column: x => x.SourceID,
                        principalTable: "Sources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeSeries_UnitTypes_UnitTypeID",
                        column: x => x.UnitTypeID,
                        principalTable: "UnitTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatagoryCoefficient_CatagoryTypeID",
                table: "CatagoryCoefficient",
                column: "CatagoryTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CatagoryCoefficient_RegionID",
                table: "CatagoryCoefficient",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_RoleID",
                table: "Managers",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_SourceID",
                table: "Permits",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_StatusTypeID",
                table: "Permits",
                column: "StatusTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_UnitTypeID",
                table: "Permits",
                column: "UnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RegionManager_RegionID",
                table: "RegionManager",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_CatagoryTypeID",
                table: "Sources",
                column: "CatagoryTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_RegionID",
                table: "Sources",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceTypeID",
                table: "Sources",
                column: "SourceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSeries_SourceID",
                table: "TimeSeries",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSeries_UnitTypeID",
                table: "TimeSeries",
                column: "UnitTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatagoryCoefficient");

            migrationBuilder.DropTable(
                name: "Permits");

            migrationBuilder.DropTable(
                name: "RegionManager");

            migrationBuilder.DropTable(
                name: "TimeSeries");

            migrationBuilder.DropTable(
                name: "StatusTypes");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "UnitTypes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CatagoryTypes");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "SourceTypes");
        }
    }
}
