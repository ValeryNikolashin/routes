using System.Collections.Generic;
using Routes.Application.Internal;
using Routes.Domain;

namespace Routes.Application
{
    public sealed class WaysBuilder
    {
        private readonly OptimalWayBuilder cheapestWayBuilder;
        private readonly OptimalWayBuilder fastestWayBuilder;

        public WaysBuilder(IEnumerable<BusRoute> routes)
        {
            cheapestWayBuilder = new CheapestOptimalWayBuilder(routes);
            fastestWayBuilder = new FastestOptimalWayBuilder(routes);
        }

        public OptimalWaysReport Build(int departureStop, int arrivalStop, string departureTime)
        {
            var cheapestWay = cheapestWayBuilder.BuildWay(departureStop, arrivalStop, departureTime);
            var fastestWay = fastestWayBuilder.BuildWay(departureStop, arrivalStop, departureTime);
            return new OptimalWaysReport(cheapestWay, fastestWay);
        }
    }
}