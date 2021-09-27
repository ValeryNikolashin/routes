namespace Routes.Domain
{
    public class OptimalWaysReport
    {
        public Way CheapestWay { get; }
        public Way FastestWay { get; }

        public OptimalWaysReport(Way cheapestWay, Way fastestWay)
        {
            CheapestWay = cheapestWay;
            FastestWay = fastestWay;
        }

        public override string ToString()
        {
            return $"The cheapest way:\n" +
                   $"{CheapestWay}\n" +
                   $"The fastest way:\n" +
                   $"{FastestWay}";
        }
    }
}