using System.Collections.Generic;
using System.Linq;
using Routes.Domain;

namespace Routes.Application.Internal
{
    internal class CheapestOptimalWayBuilder : OptimalWayBuilder
    {
        public CheapestOptimalWayBuilder(IEnumerable<BusRoute> routes) : base(routes)
        {
            
        }

        protected override IEnumerable<BusRoute> GetIndirectRoutesWhichMayBeOptimal(
            IEnumerable<BusRoute> routesPassingThroughDepartureStop,
            IEnumerable<BusRoute> directRoutes,
            Way optimalDirectWay,
            int departureTime
            )
        {
            var cheapestIndirectRoutes = routesPassingThroughDepartureStop
                .Except(directRoutes)
                .Where(x => x.Cost < optimalDirectWay.Cost);

            return cheapestIndirectRoutes;
        }

        protected override Way GetOptimalDirectWay(IEnumerable<BusRoute> directRoutes, int departureStop, int arrivalStop, int departureTime)
        {
            return directRoutes
                .Select(route => BuildDirectWay(route, departureStop, arrivalStop, departureTime))
                .Where(x => !TimeIsOver(x.ArrivalTime))
                .OrderBy(x=>x.Cost).FirstOrDefault();
        }

        protected override bool IsFirstWayWithRouteBetterThanSecondImpl(Way first, BusRoute route, Way second)
        {
            return first.Cost + route.Cost < second.Cost;
        }
    }
}