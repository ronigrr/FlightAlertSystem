using FlightAlertSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightAlertSystem.Data
{
    public class FlightAlertContext : DbContext
    {
        public FlightAlertContext(DbContextOptions<FlightAlertContext> options) : base(options) { }

        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alert>(entity =>
            {
                // Configure FlightId as optional
                entity.Property(e => e.FlightId).IsRequired(false);
            });
        }
    }
}