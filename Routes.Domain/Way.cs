using System.Collections.Generic;
using System.Linq;
using TimeConverter = Routes.Domain.Utils.TimeConverter;

namespace Routes.Domain
{
    public class Way
    {
        private int DepartureTime => Trips.Min(x=>x.DepartureTime);
        public List<Trip> Trips { get; }
        public int Cost => Trips.Sum(x => x.Cost);
        public int ArrivalTime => Trips.Max(x=>x.ArrivalTime);

        public Way(List<Trip> trips)
        {
            Trips = trips;
        }

        public override string ToString()
        {
            string tripsAsString = null;
            foreach (var trip in Trips)
            {
                tripsAsString += $"{trip}\n";
            }
            
            return
                $"Departure time: {TimeConverter.MinutesToTime(DepartureTime)}. Arrival time: {TimeConverter.MinutesToTime(ArrivalTime)}. Total cost: {Cost}.\n"
                + tripsAsString;
        }
    }
}