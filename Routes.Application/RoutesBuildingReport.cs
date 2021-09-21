using Routes.Domain;

namespace Routes.Application
{
    public class RoutesBuildingReport
    {
        public BuiltRoute CheapestRoute { get; }
        public BuiltRoute FastestRoute { get; }

        public RoutesBuildingReport(BuiltRoute cheapestRoute, BuiltRoute fastestRoute)
        {
            CheapestRoute = cheapestRoute;
            FastestRoute = fastestRoute;
        }

        public override string ToString()
        {
            return $"Самый дешёвый маршрут:\n" +
                   CheapestRoute + "\n" +
                   $"Самый быстрый маршрут:\n" +
                   FastestRoute;
        }
    }
}