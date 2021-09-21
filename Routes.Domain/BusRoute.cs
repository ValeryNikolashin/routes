using System.Collections.Generic;
using System.Linq;
using static System.Int32;

namespace Routes.Domain
{
    public sealed class BusRoute
    {
        public int Number { get; }
        public int Price { get; }
        public int DepartureTime { get; }
        public List<int> Stops { get; }
        public List<int> Intervals { get; }

        public BusRoute(int number, int price, int departureTime, List<int> stops, List<int> intervals)
        {
            Number = number;
            Price = price;
            Stops = stops;
            Intervals = intervals;
            DepartureTime = departureTime;
        }

        public int GetArrivalTime(int stop, int time)
        {
            if (!Stops.Contains(stop))
                return MaxValue;

            var firstArrivalTime = GetFirstArrivalTime(stop);
            var routeTime = Intervals.Sum(x => x);
            var arrivalTime = firstArrivalTime;

            while (arrivalTime<time+routeTime)
            {
                arrivalTime += routeTime;
            }

            return arrivalTime;
        }

        private int GetFirstArrivalTime(int stop)
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