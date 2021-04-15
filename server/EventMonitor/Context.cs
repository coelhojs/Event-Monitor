using EventMonitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventMonitor
{
    public class Context : DbContext
    {
        public DbSet<Event> Event { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            optionsBuilder.UseNpgsql(connectionString, options => options.SetPostgresVersion(new Version(9, 6)));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>()
                .HasKey(u => new { u.Timestamp, u.Region, u.Sensor, u.Value });

            builder.Entity<Event>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}