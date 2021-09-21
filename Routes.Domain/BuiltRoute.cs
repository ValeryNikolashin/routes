namespace Routes.Domain
{
    public sealed class BuiltRoute
    {
        public int RouteNumber { get; }
        public int BeginningStop { get; }
        public int EndingStop { get; }
        public int Price { get; }
        public int TimeInWay { get; }

        public BuiltRoute(int routeNumber, int beginningStop, int endingStop, int price, int timeInWay)
        {
            RouteNumber = routeNumber;
            BeginningStop = beginningStop;
            EndingStop = endingStop;
            Price = price;
            TimeInWay = timeInWay;
        }

        public override string ToString()
        {
            return
                $"Поездка {BeginningStop} => {EndingStop}, номер автобуса: {RouteNumber}, стоимость: {Price}, время в пути: {TimeInWay} мин.\n";
        }
    }
}