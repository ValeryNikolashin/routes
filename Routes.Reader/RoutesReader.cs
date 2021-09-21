using System.Collections.Generic;
using System.IO;
using System.Linq;
using Routes.Domain;
using Routes.Utils;

namespace Routes.Reader
{
    public static class RoutesReader
    {
        public static IEnumerable<BusRoute> Read(string routesFilePath)
        {
            using var routesFile = new StreamReader(routesFilePath);
            var routesCount = int.Parse(routesFile.ReadLine());
            var stopsTotalCount = int.Parse(routesFile.ReadLine());

            var departureTimeList = GetDepartureTimeListFrom(routesFile.ReadLine()).ToArray();
            var priceList = GetPriceListFrom(routesFile.ReadLine()).ToArray();

            var routes = new List<BusRoute>(routesCount);

            for (var routeNumber = 0; routeNumber < routesCount; routeNumber++)
            {
                var routeStopsInfo = routesFile.ReadLine().Split(' ');
                var routeStopsCount = int.Parse(routeStopsInfo[0]);
                var routeStops = new List<int>(routeStopsCount);
                var routeStopIntervals = new List<int>(routeStopsCount);
                    
                for (var stopIndex = 1; stopIndex <= routeStopsCount; stopIndex++)
                {
                    var stop = int.Parse(routeStopsInfo[stopIndex]);
                    routeStops.Add(stop);
                        
                    var intervalIndex = stopIndex + routeStopsCount;
                    var interval = int.Parse(routeStopsInfo[intervalIndex]);
                    routeStopIntervals.Add(interval);
                }

                routes.Add(new BusRoute(
                    routeNumber + 1,
                    priceList[routeNumber],
                    departureTimeList[routeNumber],
                    routeStops,
                    routeStopIntervals)
                );
            }

            return routes;
        }

        private static IEnumerable<int> GetPriceListFrom(string source)
        {
            return source.Split(' ').Select(int.Parse);
        }

        private static IEnumerable<int> GetDepartureTimeListFrom(string source)
        {
            var departureTimeList = source.Split(' ');

            return departureTimeList.Select(TimeConverter.TimeToMinutes);
        }
    }
}