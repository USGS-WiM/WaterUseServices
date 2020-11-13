using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WaterUseDB;

namespace WaterUseDB.Migrations
{
    [DbContext(typeof(WaterUseDBContext))]
    [Migration("20170508153801_modHasIndex")]
    partial class modHasIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("WaterUseDB.Resources.CategoryCoefficient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryTypeID");

                    b.Property<string>("Comments");

                    b.Property<int>("RegionID");

                    b.Property<double>("Value");

                    b.HasKey("ID");

                    b.HasIndex("CategoryTypeID");

                    b.HasIndex("RegionID");

                    b.ToTable("CategoryCoefficient");
                });

            modelBuilder.Entity("WaterUseDB.Resources.CategoryType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Code");

                    b.ToTable("CategoryTypes");
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

                    b.Property<string>("SecondaryPhone");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("Username");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Permit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<double>("IntakeCapacity");

                    b.Property<int>("SourceID");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("StatusTypeID");

                    b.Property<int>("UnitTypeID");

                    b.Property<double>("WellCapacity");

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

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ShortName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("ShortName");

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

                    b.HasIndex("Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WaterUseDB.Resources.Source", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryTypeID");

                    b.Property<string>("FacilityName")
                        .IsRequired();

                    b.Property<int>("RegionID");

                    b.Property<int>("SourceTypeID");

                    b.Property<string>("StationID");

                    b.HasKey("ID");

                    b.HasIndex("CategoryTypeID");

                    b.HasIndex("RegionID");

                    b.HasIndex("SourceTypeID");

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

                    b.HasIndex("Code");

                    b.ToTable("SourceTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.StatusType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Code");

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

                    b.Property<string>("Abbreviation");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Abbreviation");

                    b.ToTable("UnitTypes");
                });

            modelBuilder.Entity("WaterUseDB.Resources.CategoryCoefficient", b =>
                {
                    b.HasOne("WaterUseDB.Resources.CategoryType", "CategoryType")
                        .WithMany()
                        .HasForeignKey("CategoryTypeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.Region", "Region")
                        .WithMany("CategoryCoefficients")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WaterUseDB.Resources.Manager", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WaterUseDB.Resources.Permit", b =>
                {
                    b.HasOne("WaterUseDB.Resources.Source", "Source")
                        .WithMany("Permits")
                        .HasForeignKey("SourceID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.StatusType", "StatusType")
                        .WithMany()
                        .HasForeignKey("StatusTypeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.UnitType", "UnitType")
                        .WithMany()
                        .HasForeignKey("UnitTypeID")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("WaterUseDB.Resources.CategoryType", "CategoryType")
                        .WithMany()
                        .HasForeignKey("CategoryTypeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.Region", "Region")
                        .WithMany("Sources")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WaterUseDB.Resources.SourceType", "SourceType")
                        .WithMany()
                        .HasForeignKey("SourceTypeID")
                        .OnDelete(DeleteBehavior.Cascade);
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
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
