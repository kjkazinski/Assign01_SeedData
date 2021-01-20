using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assign01_SeedData
{
    public class LocationsEventsContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }

        public void AddLocation(Location location)
        {
            this.Locations.Add(location);
            this.SaveChanges();
        }

        public void AddEvent(Event location_event)
        {
            this.Events.Add(location_event);
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["LocationsEventsContext:ConnectionString"]);        }
    }
}