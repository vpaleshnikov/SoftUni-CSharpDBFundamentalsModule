﻿using Microsoft.EntityFrameworkCore;
using Stations.Models;

namespace Stations.Data
{
	public class StationsDbContext : DbContext
	{
		public StationsDbContext()
		{
		}

		public StationsDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Station> Stations { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<TrainSeat> TrainSeats { get; set; }

        public DbSet<SeatingClass> SeatingClasses { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<CustomerCard> Cards { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Station>()
                .HasAlternateKey(s => s.Name);

            modelBuilder.Entity<Train>()
                .HasAlternateKey(t => t.TrainNumber);

            modelBuilder.Entity<SeatingClass>()
                .HasAlternateKey(sc => new { sc.Name, sc.Abbreviation });

            modelBuilder.Entity<Trip>()
                .HasOne(e => e.DestinationStation)
                .WithMany(ds => ds.TripsTo)
                .HasForeignKey(e => e.DestinationStationId);

            modelBuilder.Entity<Trip>()
                .HasOne(e => e.OriginStation)
                .WithMany(os => os.TripsFrom)
                .HasForeignKey(e => e.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
	}
}