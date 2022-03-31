namespace GetTime
{
    class Main_mod
    {
        static void Main()
        {
            //Номер станции в системе Яндекс.Расписания
            //Station ID from yandex.schedules
            string station = "s9602496";
            string station2 = "s9602675";
            string station3 = "s9602668";
            string date;
            Console.WriteLine("Ввведите дату в формате YYYY-MM-DD");
            date = Console.ReadLine();
            DateTime enteredDate = DateTime.Parse(date);
            //Если введеная дата раньше сегодня, закрываемся
            //If entered date is earlier than today we finish or show an error
            //Выбор Отправление или прибытие
            //Choose departure or arrival
            bool dest = false;


            Console.WriteLine("Получаем данные от яндекса (Это займет пару секунд)");
            //Collecting data from yandex (this will take a couple of seconds)

            List<string> schedule = Schedule.station_timetable_get(date, station, dest);

            foreach (string scheduleItem in schedule) { Console.WriteLine(scheduleItem); }

            //Schedule.get_stationID("58.816280", "30.329888");
            List<string> route = Schedule.route_time_get(date, station, station2);
            foreach (string routeItem in route) { Console.WriteLine(routeItem); }
        }
    }
}
