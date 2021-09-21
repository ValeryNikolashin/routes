namespace Routes.Utils
{
    public static class TimeConverter
    {
        public static int TimeToMinutes(string time)
        {
            var splitTime = time.Split(':');
            var hours = int.Parse(splitTime[0]);
            var minutes = int.Parse(splitTime[1]);

            return hours * 60 + minutes;
        }

        public static bool TimeIsOver(int arrivalTime)
        {
            const int maxTimeInMinutes = 60 * 24;
            return arrivalTime > maxTimeInMinutes;
        }
    }
}