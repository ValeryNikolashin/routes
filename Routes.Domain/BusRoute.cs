using System.Collections.Generic;
using System.Linq;
using static System.Int32;

namespace Routes.Domain
{
    public sealed class BusRoute
    {
        public int Number { get; }
        public int Cost { get; }
        private int DepartureTime { get; }
        public List<int> Stops { get; }
        private List<int> Intervals { get; }

        public BusRoute(int number, int cost, int departureTime, List<int> stops, List<int> intervals)
        {
            Number = number;
            Cost = cost;
            Stops = stops;
            Intervals = intervals;
            DepartureTime = departureTime;
        }

        public int GetArrivalTime(int toStop, int desiredArrivalTime)
        {
            if (!Stops.Contains(toStop))
                return MaxValue;

            var firstArrivalTime = GetFirstArrivalTimeToStop(toStop);
            var routeTime = Intervals.Sum(x => x);
            var arrivalTime = firstArrivalTime;

            while (arrivalTime<desiredArrivalTime)
            {
                arrivalTime += routeTime;
            }

            return arrivalTime;
        }
        
        public int GetArrivalTime(int fromStop, int toStop, int desiredDepartureTime)
        {
            var realDepartureTime = GetArrivalTime(fromStop, desiredDepartureTime);

            return GetArrivalTime(toStop, realDepartureTime);
        }

        private int GetFirstArrivalTimeToStop(int stop)
        {
            var i = 0;
            var arrivalTime = DepartureTime;
            while (Stops[i] != stop)
            {
                arrivalTime += Intervals[i];
                i++;
            }

            return arrivalTime;
        }
    }
}