using System;
using ERNST.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ERNST.Data
{
    public partial class ERNSTContext : DbContext
    {
        public ERNSTContext()
        {
        }

        public ERNSTContext(DbContextOptions<ERNSTContext> options)
            : base(options)
        {
        }

        public DbSet<TrainType> TrainTypes { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<CoachClass> CoachClass { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<WorkshopCoach> WorkshopCoaches { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<CompositionRecord> CompositionRecords { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Stations> Stations { get; set; }
        public DbSet<LineStations> LineStations { get; set; }
        public DbSet<Lines> Lines { get; set; }
        public DbSet<TrainSchedule> TrainSchedule { get; set; }
        public DbSet<Schedule> Time_Table { get; set; }
        public DbSet<Time_Tabale_Stations> Time_Table_Stations { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<TicketTypes> TicketTypes { get; set; }
        public DbSet<PassengersType> PassengersType { get; set; }
        public DbSet<Governorate> Governorate { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Gov_Regions> Gov_Regions { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //    //optionsBuilder.UseNpgsql("Host=localhost;Database=ERNST;Username=postgres;Password=123@@##Mm");
            //    optionsBuilder.UseNpgsql("18.224.183.46;Database=ernst;Username=postgres;Password=12345678");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {}
    }
}
