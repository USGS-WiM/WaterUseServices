//------------------------------------------------------------------------------
//----- DB Context ---------------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   Resonsible for interacting with Database 
//
//discussion:   The primary class that is responsible for interacting with data as objects. 
//              The context class manages the entity objects during run time, which includes 
//              populating objects with data from a database, change tracking, and persisting 
//              data to the database.
//              
//
//   

using Microsoft.EntityFrameworkCore;
using WaterUseDB.Resources;
using Microsoft.EntityFrameworkCore.Metadata;
//specifying the data provider and connection string
namespace WaterUseDB
{
    public class WaterUseDBContext:DbContext
    {
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<CategoryCoefficient> CategoryCoefficients { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<TimeSeries> TimeSeries { get; set; }
        public DbSet<UseType> UseTypes { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public WaterUseDBContext() : base()
        {
        }
        public WaterUseDBContext(DbContextOptions<WaterUseDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //unique key based on region and manager keys
            modelBuilder.Entity<RegionManager>().HasKey(k => new { k.ManagerID, k.RegionID });

            //Specify other unique constraints
            //EF Core currently does not support changing the value of alternate keys. We do have #4073 tracking removing this restriction though.
            //BTW it only needs to be an alternate key if you want it to be used as the target key of a relationship.If you just want a unique index, then use the HasIndex() method, rather than AlternateKey().Unique index values can be changed.
            modelBuilder.Entity<SourceType>().HasIndex(k => k.Code).IsUnique();
            modelBuilder.Entity<UseType>().HasIndex(k => k.Code).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(k => k.Name).IsUnique();
            modelBuilder.Entity<Manager>().HasIndex(k => k.Username).IsUnique();
            modelBuilder.Entity<Region>().HasIndex(k => k.ShortName).IsUnique();
            modelBuilder.Entity<CategoryType>().HasIndex(k => k.Code).IsUnique();
            modelBuilder.Entity<UnitType>().HasIndex(k => k.Abbreviation).IsUnique();
            modelBuilder.Entity<StatusType>().HasIndex(k => k.Code).IsUnique();
            modelBuilder.Entity<Source>().HasIndex(k => k.FacilityCode).IsUnique();

            //add shadowstate for when models change
            foreach (var entitytype in modelBuilder.Model.GetEntityTypes())
            {
                //modelBuilder.Entity(entitytype.Name).Property<DateTime>("LastModified");
            }//next entitytype

            //cascade delete is default, rewrite behavior
            modelBuilder.Entity("WaterUseDB.Resources.CategoryCoefficient", b =>
            {
                b.HasOne("WaterUseDB.Resources.CategoryType", "CategoryType")
                    .WithMany()
                    .HasForeignKey("CategoryTypeID")
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity("WaterUseDB.Resources.Manager", b =>
            {
                b.HasOne("WaterUseDB.Resources.Role", "Role")
                    .WithMany()
                    .HasForeignKey("RoleID")
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity("WaterUseDB.Resources.Source", b =>
                {
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
                b.HasOne("WaterUseDB.Resources.UnitType", "UnitType")
                    .WithMany()
                    .HasForeignKey("UnitTypeID")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);             
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning Add connectionstring for migrations
            var connectionstring = "User ID=;Password=;Host=;Port=5432;Database=wateruse;Pooling=true;";
            //optionsBuilder.UseNpgsql(connectionstring);
        }
    }
}
