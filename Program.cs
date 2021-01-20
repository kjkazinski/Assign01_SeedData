using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace Assign01_SeedData
{
    class Program
    {
        static void Main(string[] args)
        {
            // Because I don't like magic numbers
            const int SECONDS_PER_DAY = 86400; // 60 seconds * 60 minutes * 24 hours

            // Create the database connection            
            var db = new LocationsEventsContext();

            // Add the locations if they don't exist    
            int LocationCount = db.Locations.Count();
            Console.WriteLine("Locations: " + LocationCount);
            if (LocationCount < 3)
            {
                db.AddLocation(new Location { Name = "Family Room" });
                db.AddLocation(new Location { Name = "Front Door" });
                db.AddLocation(new Location { Name = "Rear Door" });
                
                // Write out the number of locations after the update
                LocationCount = db.Locations.Count();
                Console.WriteLine("Locations After Update: {0}", LocationCount);                
            };

            // Get the locations, the var will be converted to a list
            var Locations = (from Location in db.Locations select Location).ToList();

            // Show the location ids exist
            foreach(var Location in Locations)
                Console.WriteLine("Location ID = {0}, Name = {1}", Location.LocationId, Location.Name);

            // Set the staring date 6 months ago
            DateTime Today = DateTime.Now.Date;
            Console.WriteLine ("Date Today: {0:R}", Today);
            DateTime ProcessDate = Today.AddMonths(-6);
            Console.WriteLine ("Date Start: {0:R}", ProcessDate);
            DateTime LocationEventDateTime;

            // Create the variables needed for the events
            Random RandomNum = new Random();
            int NumberOfEvents;
            int lcv; // Loop control variable
            int Index;

            do
            {
                NumberOfEvents = RandomNum.Next(0,6);
                Console.WriteLine ("Processing {0} Events for    Date: {1:R}", NumberOfEvents, ProcessDate);

                // By setting lcv = 1, the loop will be skipped for zero events
                for (lcv = 1; lcv <= NumberOfEvents; lcv++)
                {
                    //Console.WriteLine ("\tProcessing Event {0} for Date: {1:d}", lcv, ProcessDate);
                    
                    Index = RandomNum.Next(LocationCount);
                    LocationEventDateTime = ProcessDate.AddSeconds(RandomNum.Next(SECONDS_PER_DAY));
                    Console.WriteLine ("\tProcessing Event {0} Date: {1:R} Location {2} - {3}", lcv, LocationEventDateTime, Locations[Index].LocationId, Locations[Index].Name );

                    Event NewEvent = new Event{TimeStamp = LocationEventDateTime,
                                               LocationId = Locations[Index].LocationId,
                                               Location = Locations[Index]};

                    db.AddEvent(NewEvent);
                }

                ProcessDate = ProcessDate.AddDays(1);
            } while (ProcessDate <= Today);
        }
    }
}
