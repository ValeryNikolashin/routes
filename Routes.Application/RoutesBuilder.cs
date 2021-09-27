using System;
using System.Collections.Generic;
using System.Linq;
using Routes.Domain;
using Routes.Utils;

namespace Routes.Application
{
    public sealed class RoutesBuilder
    {
        private readonly IEnumerable<BusRoute> routes;

        public RoutesBuilder(IEnumerable<BusRoute> routes)
        {
            this.routes = routes;
        }

        public Way BuildCheapestWay(int departureStop, int arrivalStop, string departureTime)
        {
            var departureTimeInMinutes = TimeConverter.TimeToMinutes(departureTime);
            
            var routesPassingThroughDepartureStop = GetRoutesPassingThroughDepartureStop(routes, departureStop).ToList();

            var directRoutes = GetDirectRoutes(routesPassingThroughDepartureStop, departureStop, arrivalStop).ToList();
            var cheapestDirectWay = GetCheapestDirectWay(directRoutes, departureStop, arrivalStop, departureTimeInMinutes);
            var cheapestDirectWayCoast = cheapestDirectWay.Cost;

            var indirectRoutes = routesPassingThroughDepartureStop
                .Except(directRoutes)
                .Where(x => x.Cost < cheapestDirectWayCoast);

            var cheapestIndirectWay = GetCheapestIndirectWay(
                indirectRoutes,
                departureStop,
                arrivalStop,
                departureTimeInMinutes,
                cheapestDirectWayCoast
            );

            return cheapestIndirectWay ?? cheapestDirectWay;
        }


        private Way GetCheapestIndirectWay(
            IEnumerable<BusRoute> indirectRoutes,
            int departureStop,
            int arrivalStop,
            int departureTime,
            int cheapestCoast
        )
        {
            if (indirectRoutes == null) return null;

            var cheapestIndirectWays = new List<Way>();

            foreach (var indirectRoute in indirectRoutes)
            {
                foreach (var stop in indirectRoute.Stops)
                {
                    if (stop == departureStop) continue;

                    var routesPassingThroughCurrentStop = GetRoutesPassingThroughDepartureStop(routes, stop)
                        .Where(x => x != indirectRoute);
                    
                    var realDepartureTime = indirectRoute.GetArrivalTime(departureStop, departureTime);
                    var arrivalTimeToCurrentStop = indirectRoute.GetArrivalTime(departureStop, stop, departureTime);

                    var directRoutes = GetDirectRoutes(routes, stop, arrivalStop).ToList();
                    var cheapestDirectWayForCurrentStop = GetCheapestDirectWay(
                        directRoutes,
                        stop,
                        arrivalStop,
                        arrivalTimeToCurrentStop
                    );
                    if (cheapestDirectWayForCurrentStop != null)
                        cheapestCoast = Math.Min(cheapestCoast, cheapestDirectWayForCurrentStop.Cost);
                    var coast = cheapestCoast;
                    
                    var indirectRoutesForCurrentStop = routesPassingThroughCurrentStop.Except(directRoutes)
                        .Where(x => x.Cost < coast);
                    var cheapestIndirectWayForCurrentStop = GetCheapestIndirectWay(
                        indirectRoutesForCurrentStop,
                        stop,
                        arrivalStop,
                        arrivalTimeToCurrentStop,
                        cheapestCoast
                    );

                    if (cheapestDirectWayForCurrentStop != null)
                    {
                        cheapestDirectWayForCurrentStop.Trips.Add(new Trip(
                            indirectRoute.Number,
                            departureStop,
                            realDepartureTime,
                            stop,
                            arrivalTimeToCurrentStop,
                            indirectRoute.Cost)
                        );

                        cheapestIndirectWays.Add(cheapestDirectWayForCurrentStop);
                    }
                    
                    if (cheapestIndirectWayForCurrentStop != null)
                    {
                        cheapestIndirectWayForCurrentStop.Trips.Add(new Trip(
                            indirectRoute.Number,
                            departureStop,
                            realDepartureTime,
                            stop,
                            arrivalTimeToCurrentStop,
                            indirectRoute.Cost)
                        );

                        cheapestIndirectWays.Add(cheapestIndirectWayForCurrentStop);
                    }
                }
            }

            var cheapestIndirectWayCost = cheapestIndirectWays.Any() ? cheapestIndirectWays.Min(y => y.Cost) : 0;
            
            var cheapestIndirectWay = cheapestIndirectWays.FirstOrDefault(x => x.Cost == cheapestIndirectWayCost);
            cheapestIndirectWay?.Trips.Reverse();

            return cheapestIndirectWays.FirstOrDefault(x => x.Cost == cheapestIndirectWayCost);
        }

        private static Way GetCheapestDirectWay(IEnumerable<BusRoute> directRoutes, int departureStop, int arrivalStop,
            int departureTime)
        {
            return directRoutes
                .Select(route => BuildDirectWay(route, departureStop, arrivalStop, departureTime))
                .Where(x => !TimeIsOver(x.ArrivalTime))
                .OrderBy(x=>x.Cost).FirstOrDefault();
        }

        private static IEnumerable<BusRoute> GetDirectRoutes(IEnumerable<BusRoute> directRoutes, int departureStop,
            int arrivalStop)
        {
            return directRoutes.Where(x => x.Stops.Contains(departureStop) && x.Stops.Contains(arrivalStop));
        }

        private static IEnumerable<BusRoute> GetRoutesPassingThroughDepartureStop(IEnumerable<BusRoute> directRoutes,
            int departureStop)
        {
            return directRoutes.Where(x => x.Stops.Contains(departureStop));
        }

        private static Way BuildDirectWay(BusRoute busRoute, int departureStop, int arrivalStop, int departureTime)
        {
            var realDepartureTime = busRoute.GetArrivalTime(departureStop, departureTime);
            var arrivalTime = busRoute.GetArrivalTime(departureStop, arrivalStop, departureTime);
            var cost = arrivalTime != int.MaxValue ? busRoute.Cost : int.MaxValue;

            return new Way(
                new List<Trip>
                {
                    new(busRoute.Number, departureStop,  realDepartureTime, arrivalStop, arrivalTime, cost)
                }
            );
        }

        private static bool TimeIsOver(int arrivalTime)
        {
            const int maxTimeInMinutes = 60 * 24;
            return arrivalTime > maxTimeInMinutes;
        }
    }
}