using System.Collections.Generic;
using System.Linq;
using Routes.Domain;

namespace Routes.Application.Internal
{
    internal class FastestOptimalWayBuilder : OptimalWayBuilder
    {
        public FastestOptimalWayBuilder(IEnumerable<BusRoute> routes) : base(routes)
        {
        }

        protected override IEnumerable<BusRoute> GetIndirectRoutesWhichMayBeOptimal(
            IEnumerable<BusRoute> routesPassingThroughDepartureStop,
            IEnumerable<BusRoute> directRoutes,
            Way optimalDirectWay,
            int departureTime)
        {
            var fastestIndirectRoutes = routesPassingThroughDepartureStop
                .Except(directRoutes)
                .Where(x => x.Stops.Any(stop=>x.GetArrivalTime(stop, departureTime) < optimalDirectWay.ArrivalTime));

            return fastestIndirectRoutes;
        }

        protected override bool IsFirstWayWithRouteBetterThanSecondImpl(Way first, BusRoute route, Way second)
        {
            return first.ArrivalTime < second.ArrivalTime;
        }

        protected override Way GetOptimalDirectWay(IEnumerable<BusRoute> directRoutes, int departureStop, int arrivalStop, int departureTime)
        {
            return directRoutes
                .Select(route => BuildDirectWay(route, departureStop, arrivalStop, departureTime))
                .Where(x => !TimeIsOver(x.ArrivalTime))
                .OrderBy(x=>x.ArrivalTime).FirstOrDefault();
        }
    }
}