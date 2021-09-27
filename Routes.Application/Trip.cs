using Routes.Utils;

namespace Routes.Application
{
    public class Trip
    {
        private int RouteNumber { get; }
        private int DepartureStop { get; }
        private int ArrivalStop { get; }
        public int DepartureTime { get; }
        public int ArrivalTime { get; }
        public int Cost { get; }

        public Trip(int routeNumber, int departureStop, int departureTime, int arrivalStop, int arrivalTime, int cost)
        {
            RouteNumber = routeNumber;
            DepartureStop = departureStop;
            DepartureTime = departureTime;
            ArrivalStop = arrivalStop;
            ArrivalTime = arrivalTime;
            Cost = cost;
        }

        public override string ToString()
        {
            return
                $"Route {RouteNumber}, cost {Cost} rubles. From {DepartureStop} in {TimeConverter.MinutesToTime(DepartureTime)} to {ArrivalStop} in {TimeConverter.MinutesToTime(ArrivalTime)}.";
        }
    }
}