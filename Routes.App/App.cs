using System;
using Routes.API;

namespace Routes
{
    class App
    {
        private static RoutesController controller;
        static void Main(string[] args)
        {
            Console.WriteLine("Программа для поиска оптимальных маршрутов автобусов.");
            while (true)
            {
                ReadRoutes();
                BuildOptimalRoutes();
            }
        }

        private static void ReadRoutes()
        {
            Console.WriteLine("Введите путь к файлу с маршрутами:");
            var routesFilePath = Console.ReadLine();
            controller = new RoutesController(routesFilePath);
            Console.WriteLine("Маршруты успешно загружены.");
        }

        private static void BuildOptimalRoutes()
        {
            while (true)
            {
                Console.WriteLine("Для построения оптимальных маршрутов, введите исходную и конечную остановку и время отправления.");
                Console.Write("Исходная остановка: ");
                var beginningStop = int.Parse(Console.ReadLine());
                Console.Write("Конечная остановка: ");
                var endingStop = int.Parse(Console.ReadLine());
                Console.Write("Время отправления (hh:mm): ");
                var departureTime = Console.ReadLine();
                var cheapestWay = controller.BuildOptimalRoutes(beginningStop, endingStop, departureTime);
                Console.WriteLine(cheapestWay);
                Console.WriteLine("Для поиска нового маршрута, введите любой символ.");
                Console.WriteLine("Для загрузки нового файла маршрутов, введите 0.");
                var menu = Console.ReadLine();
                if(menu == "0") break;
            }
        }
    }
}