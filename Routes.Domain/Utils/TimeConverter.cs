namespace Routes.Domain.Utils
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
        
        public static string MinutesToTime(int minutes)
        {
            var timeHours = minutes / 60;
            var timeMinutes = minutes - timeHours * 60;

            return $"{timeHours}:{timeMinutes}";
        }
    }
}