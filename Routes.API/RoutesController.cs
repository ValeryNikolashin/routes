using Routes.Application;
using Routes.Domain;
using Routes.Reader;

namespace Routes.API
{
    public sealed class RoutesController
    {
        private readonly WaysBuilder _waysBuilder;

        public RoutesController(string routesFilePath)
        {
            _waysBuilder = new WaysBuilder(RoutesReader.Read(routesFilePath));
        }

        /// <summary>
        /// Строит оптимальные маршруты
        /// </summary>
        /// <param name="beginningStop">Остановка отправления</param>
        /// <param name="endingStop">Остановка прибытия</param>
        /// <param name="time">Время отправления</param>
        /// <returns>Оптимальные маршруты</returns>
        public OptimalWaysReport BuildOptimalRoutes(int beginningStop, int endingStop, string time)
        {
            return _waysBuilder.Build( beginningStop, endingStop, time);
        } 
    }
}