using Routes.Application;
using Routes.Reader;

namespace Routes.API
{
    public sealed class RoutesController
    {
        private readonly RoutesBuilder routesBuilder;

        public RoutesController(string routesFilePath)
        {
            routesBuilder = new RoutesBuilder(RoutesReader.Read(routesFilePath));
        }

        /// <summary>
        /// Строит оптимальные маршруты
        /// </summary>
        /// <param name="beginningStop">Остановка отправления</param>
        /// <param name="endingStop">Остановка прибытия</param>
        /// <param name="time">Время отправления</param>
        /// <returns>Оптимальные маршруты</returns>
        public Way BuildOptimalRoutes(int beginningStop, int endingStop, string time)
        {
            return routesBuilder.BuildCheapestWay( beginningStop, endingStop, time);
        } 
    }
}