using System.Collections.Generic;
using System.Linq;
using Routes.Domain;
using Routes.Utils;

namespace Routes.Application
{
    public static class RoutesBuilder
    {
        public static RoutesBuildingReport Build(
            IEnumerable<BusRoute> routes,
            int beginningStop,
            int endingStop,
            string time
        )
        {
            var timeInMinutes = TimeConverter.TimeToMinutes(time);
            
            var builtRoutes = GetRoutes(routes, beginningStop, endingStop, timeInMinutes).ToArray();
            var cheapestRoute = builtRoutes.OrderBy(x => x.Price).FirstOrDefault();
            var fastestRoute = builtRoutes.OrderBy(x => x.TimeInWay).FirstOrDefault();
            
            return new RoutesBuildingReport(cheapestRoute, fastestRoute);
        }

        private static IEnumerable<BuiltRoute> GetRoutes(
            IEnumerable<BusRoute> routes,
            int beginningStop,
            int endingStop,
            int timeInMinutes
        )
        {
            return routes
                .Where(x => x.Stops.Contains(beginningStop) && x.Stops.Contains(endingStop))
                .Select(route => BuildRoute(route, beginningStop, endingStop, timeInMinutes))
                .Where(x => !TimeConverter.TimeIsOver(x.TimeInWay + timeInMinutes));
        }

        private static BuiltRoute BuildRoute(BusRoute busRoute, int beginningStop, int endingStop, int timeInMinutes)
        {
            var arrivalTime = busRoute.GetArrivalTime(endingStop, timeInMinutes);
            var price = arrivalTime != int.MaxValue ? busRoute.Price : int.MaxValue;
            
            return new BuiltRoute(
                busRoute.Number,
                beginningStop,
                endingStop,
                price,
                arrivalTime - timeInMinutes
            );
        }
    }
}