using System;
using System.Collections.Generic;
using System.Linq;
using Routes.Domain;
using Routes.Domain.Utils;

namespace Routes.Application.Internal
{
    internal class OptimalWayBuilder
    {
        private readonly IEnumerable<BusRoute> routes;

        protected OptimalWayBuilder(IEnumerable<BusRoute> routes)
        {
            this.routes = routes;
        }
        
        public Way BuildWay(int departureStop, int arrivalStop, string departureTime)
        {
            var departureTimeInMinutes = TimeConverter.TimeToMinutes(departureTime);
            
            var routesPassingThroughDepartureStop = GetRoutesPassingThroughDepartureStop(routes, departureStop).ToList();

            var directRoutes = GetDirectRoutes(routesPassingThroughDepartureStop, departureStop, arrivalStop).ToList();
            var optimalDirectWay = GetOptimalDirectWay(directRoutes, departureStop, arrivalStop, departureTimeInMinutes);

            var indirectRoutes = GetIndirectRoutesWhichMayBeOptimal(
                routesPassingThroughDepartureStop,
                directRoutes,
                optimalDirectWay, departureTimeInMinutes).ToList();

            var optimalIndirectWay = GetOptimalIndirectWay(
                indirectRoutes,
                departureStop,
                arrivalStop,
                departureTimeInMinutes,
                optimalDirectWay
            );

            var optimalWay = optimalIndirectWay ?? optimalDirectWay;
            optimalWay?.Trips.Reverse();

            return optimalWay;
        }
        
        protected virtual bool IsFirstWayWithRouteBetterThanSecondImpl(Way first, BusRoute route, Way second)
        {
            throw new NotImplementedException();
        }

        protected virtual Way GetOptimalDirectWay(IEnumerable<BusRoute> directRoutes, int departureStop, int arrivalStop,
            int departureTime)
        {
            throw new NotImplementedException();
        }
        
        protected virtual IEnumerable<BusRoute> GetIndirectRoutesWhichMayBeOptimal(
            IEnumerable<BusRoute> routesPassingThroughDepartureStop,
            IEnumerable<BusRoute> directRoutes,
            Way optimalDirectWay,
            int departureTime
        )
        {
            throw new NotImplementedException();
        }

        private Way GetOptimalIndirectWay(
            IEnumerable<BusRoute> indirectRoutes,
            int departureStop,
            int arrivalStop,
            int departureTime,
            Way optimalWay
        )
        {
            if (indirectRoutes == null) return null;

            foreach (var indirectRoute in indirectRoutes)
            {
                foreach (var stop in indirectRoute.Stops)
                {
                    if (stop == departureStop) continue;

                    var routesPassingThroughCurrentStop = GetRoutesPassingThroughDepartureStop(routes, stop)
                        .Where(x => x != indirectRoute).ToList();
                    
                    var realDepartureTime = indirectRoute.GetArrivalTime(departureStop, departureTime);
                    var arrivalTimeToCurrentStop = indirectRoute.GetArrivalTime(departureStop, stop, departureTime);

                    var directRoutes = GetDirectRoutes(routes, stop, arrivalStop).ToList();
                    var optimalDirectWayForCurrentStop = GetOptimalDirectWay(
                        directRoutes,
                        stop,
                        arrivalStop,
                        arrivalTimeToCurrentStop
                    );

                    if (IsFirstWayWithRouteBetterThanSecond(optimalDirectWayForCurrentStop, indirectRoute, optimalWay))
                    {
                        optimalDirectWayForCurrentStop.Trips.Add(new Trip(
                            indirectRoute.Number,
                            departureStop,
                            realDepartureTime,
                            stop,
                            arrivalTimeToCurrentStop,
                            indirectRoute.Cost));
                        optimalWay = optimalDirectWayForCurrentStop;
                    }

                    var indirectRoutesForCurrentStop = GetIndirectRoutesWhichMayBeOptimal(
                        routesPassingThroughCurrentStop,
                        directRoutes,
                        optimalWay,
                        departureTime).ToList();
                    
                    var optimalIndirectWayForCurrentStop = GetOptimalIndirectWay(
                        indirectRoutesForCurrentStop,
                        stop,
                        arrivalStop,
                        arrivalTimeToCurrentStop,
                        optimalWay
                    );

                    if (IsFirstWayWithRouteBetterThanSecond(optimalIndirectWayForCurrentStop, indirectRoute, optimalWay))
                    {
                        optimalIndirectWayForCurrentStop.Trips.Add(new Trip(
                            indirectRoute.Number,
                            departureStop,
                            realDepartureTime,
                            stop,
                            arrivalTimeToCurrentStop,
                            indirectRoute.Cost));
                        optimalWay = optimalIndirectWayForCurrentStop;
                    }
                }
            }

            return optimalWay;
        }

        private bool IsFirstWayWithRouteBetterThanSecond(Way first, BusRoute route, Way second)
        {
            if (first == null) return false;
            return second == null || IsFirstWayWithRouteBetterThanSecondImpl(first, route, second);
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

        protected static Way BuildDirectWay(BusRoute busRoute, int departureStop, int arrivalStop, int departureTime)
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

        protected static bool TimeIsOver(int arrivalTime)
        {
            const int maxTimeInMinutes = 60 * 24;
            return arrivalTime > maxTimeInMinutes;
        }
    }
}