using System.Collections.Generic;

namespace GetTime
{
    class Main_mod
    {
        static void Main()
        {
            //Номер станции в системе Яндекс.Расписания
            //Station ID from yandex.schedules
            string date;
            Console.WriteLine("Ввведите дату в формате YYYY-MM-DD");
            date = Console.ReadLine();
            DateTime enteredDate = DateTime.Parse(date);
            Console.WriteLine("Станция 1:");
            string station1 = DB.Find_station_code(Convert.ToString(Console.ReadLine())).code;
            Console.WriteLine("Станция 2:");
            string station2 = DB.Find_station_code(Convert.ToString(Console.ReadLine())).code;
            if (station1 is null || station2 is null) 
            { 
                Console.WriteLine("По вашему запросу не найдено станций");
            }
            //Если введеная дата раньше сегодня, закрываемся
            //If entered date is earlier than today we finish or show an error
            Console.WriteLine("Получаем данные от яндекса (Это займет пару секунд)");
            //Collecting data from yandex (this will take a couple of seconds)
            List<Schedule.Route> schedule = new List<Schedule.Route>();
            schedule = Schedule.station_timetable_get(date, station1);
            foreach (Schedule.Route route in schedule)
            {
                if (route.depart != null) { Console.WriteLine("--> " + route.depart); }
                else { Console.WriteLine("<-- " + route.arrival); }
                Console.WriteLine("#####");
            }

            //Schedule.get_stationID("58.816280", "30.329888");
            List<Schedule.Route> Route_list = new List<Schedule.Route>();
            Route_list = Schedule.route_time_get(date, station1, station2);
            foreach (Schedule.Route route in Route_list) 
            {
                Console.WriteLine("####" + route.title + "####");
                Console.WriteLine(route.depart);
                Console.WriteLine(route.arrival);
            }
            //Schedule.route_get_station();
            
        }
    }
}
