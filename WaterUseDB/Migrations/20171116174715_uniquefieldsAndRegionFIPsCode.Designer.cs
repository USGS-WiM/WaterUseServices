﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using NpgsqlTypes;
using System;
using WaterUseDB;

namespace WaterUseDB.Migrations
{
    [DbContext(typeof(WaterUseDBContext))]
    [Migration("20171116174715_uniquefieldsAndRegionFIPsCode")]
    partial class uniquefieldsAndRegionFIPsCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("WaterUseDB.Resources.CatagoryCoefficient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CatagoryTypeID");

                    b.Property<string>("Comments");

                    b.Property<int>("RegionID");

                    b.Property<double>("Value");

                    b.HasKey("ID");

                    b.HasIndex("CatagoryTypeID");

                    b.HasIndex("RegionID");

                    b.ToTable("CatagoryCoefficients");
                });

            modelBuilder.Entity("WaterUseDB.Resources.CatagoryType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("CatagoryTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Manager", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("OtherInfo");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("PrimaryPhone");

                    b.Property<int>("RoleID");

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.Property<string>("SecondaryPhone");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Permit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("EndDate");

                    b.Property<double?>("IntakeCapacity");

                    b.Property<string>("PermitNO")
                        .IsRequired();

                    b.Property<int>("SourceID");

                    b.Property<DateTime?>("StartDate");

                    b.Property<int?>("StatusTypeID");

                    b.Property<int?>("UnitTypeID");

                    b.Property<double?>("WellCapacity");

                    b.HasKey("ID");

                    b.HasIndex("SourceID");

                    b.HasIndex("StatusTypeID");

                    b.HasIndex("UnitTypeID");

                    b.ToTable("Permits");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Region", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("FIPSCode")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ShortName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("ShortName")
                        .IsUnique();

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("WaterUseDB.Resources.RegionManager", b =>
                {
                    b.Property<int>("ManagerID");

                    b.Property<int>("RegionID");

                    b.HasKey("ManagerID", "RegionID");

                    b.HasIndex("RegionID");

                    b.ToTable("RegionManager");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Source", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CatagoryTypeID");

                    b.Property<string>("FacilityCode")
                        .IsRequired();

                    b.Property<string>("FacilityName")
                        .IsRequired();

                    b.Property<PostgisPoint>("Location")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<int>("RegionID");

                    b.Property<int>("SourceTypeID");

                    b.Property<string>("StationID");

                    b.Property<int>("UseTypeID");

                    b.HasKey("ID");

                    b.HasIndex("CatagoryTypeID");

                    b.HasIndex("FacilityCode")
                        .IsUnique();

                    b.HasIndex("RegionID");

                    b.HasIndex("SourceTypeID");

                    b.HasIndex("UseTypeID");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("WaterUseDB.Resources.SourceType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("SourceTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.StatusType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("StatusTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.TimeSeries", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("SourceID");

                    b.Property<int>("UnitTypeID");

                    b.Property<double>("Value");

                    b.HasKey("ID");

                    b.HasIndex("SourceID");

                    b.HasIndex("UnitTypeID");

                    b.ToTable("TimeSeries");
                });

            modelBuilder.Entity("WaterUseDB.Resources.UnitType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreviation")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Abbreviation")
                        .IsUnique();

                    b.ToTable("UnitTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.UseType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("UseTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.CatagoryCoefficient", b =>
                {
                    b.HasOne("WaterUseDB.Resources.CatagoryType", "CatagoryType")
                        .WithMany()
                        .HasForeignKey("CatagoryTypeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WaterUseDB.Resources.Region", "Region")
                        .WithMany("CatagoryCoefficients")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WaterUseDB.Resources.Manager", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WaterUseDB.Resources.Permit", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Source", "Source")
                        .WithMany("Permits")
                        .HasForeignKey("SourceID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.StatusType", "StatusType")
                        .WithMany()
                        .HasForeignKey("StatusTypeID");

                    b.HasOne("WaterUseDB.Resources.UnitType", "UnitType")
                        .WithMany()
                        .HasForeignKey("UnitTypeID");
                });

            modelBuilder.Entity("WaterUseDB.Resources.RegionManager", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Manager", "Manager")
                        .WithMany("RegionManagers")
                        .HasForeignKey("ManagerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.Region", "Region")
                        .WithMany("RegionManagers")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WaterUseDB.Resources.Source", b =>
                {
                    b.HasOne("WaterUseDB.Resources.CatagoryType", "CatagoryType")
                        .WithMany()
                        .HasForeignKey("CatagoryTypeID");

                    b.HasOne("WaterUseDB.Resources.Region", "Region")
                        .WithMany("Sources")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WaterUseDB.Resources.SourceType", "SourceType")
                        .WithMany()
                        .HasForeignKey("SourceTypeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WaterUseDB.Resources.UseType", "UseType")
                        .WithMany()
                        .HasForeignKey("UseTypeID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WaterUseDB.Resources.TimeSeries", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Source", "Source")
                        .WithMany("TimeSeries")
                        .HasForeignKey("SourceID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.UnitType", "UnitType")
                        .WithMany()
                        .HasForeignKey("UnitTypeID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
