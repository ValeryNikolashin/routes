using System.Collections.Generic;
using Routes.Application;
using Routes.Domain;
using Routes.Reader;

namespace Routes.API
{
    public sealed class RoutesController
    {
        private IEnumerable<BusRoute> Routes { get; set; }

        /// <summary>
        /// Считывает маршруты из файла в память
        /// </summary>
        /// <param name="routesFilePath">Путь к файлу с маршрутами</param>
        public void InitRoutesFromFile(string routesFilePath)
        {
            Routes = RoutesReader.Read(routesFilePath);
        }

        /// <summary>
        /// Строит оптимальные маршруты
        /// </summary>
        /// <param name="beginningStop">Остановка отправления</param>
        /// <param name="endingStop">Остановка прибытия</param>
        /// <param name="time">Время отправления</param>
        /// <returns>Оптимальные маршруты</returns>
        public RoutesBuildingReport BuildOptimalRoutes(int beginningStop, int endingStop, string time)
        {
            return RoutesBuilder.Build(Routes, beginningStop, endingStop, time);
        } 
    }
}