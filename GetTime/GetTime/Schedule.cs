using HttpUtils;
using Newtonsoft.Json.Linq;


namespace GetTime
{
    class Schedule
    {
        class Auth
        // Here we can pass the uid and api key 
        // uid is needed to check the stations on route and to check what is the next station
        // Сюда мы передаем ключ от api и uid
        // uid используется чтобы проверить станции по маршруту и проверить какая следующая станция
        {
            public string uid = "6453_0_9602496_g22_4";
            private string api_key;
            
            public string key
            {
                get { return "970ac53c-a84f-4818-99a7-a52b5ceff6b2"; }
                set { }
            }
        }
        public struct Route 
        { 
            public string depart;
            public string title;
            public string arrival;
        };
        public List<string> stations;
        public static List<Route> station_timetable_get(string date, string station)
        //This is the json parcer so it basicall gets an answer from the api and gets the needed data
        {
            Auth auth = new Auth();
            //Лист для возврата расписания
            //A list to return the schedule in list
            List<Route> schedule = new List<Route>();
            string api_point = $"https://api.rasp.yandex.net/v3.0/schedule/?apikey={auth.key}&station={station}&transport_types=suburban&date={date}";
            var client = new RestClient();

            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";
            //Sending a request
            //Отправляем запрос
            var json = client.MakeRequest();
            //Parcing the answer
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = 98;
            try
            {
                //We are collecting all the recordings and move them to list
                //В цикле собираем все записи и переносим их в вид списка
                for (int i = 0; i < length; i++)
                {
                    var route = new Route();
                    if (responce.schedule[i].direction == "прибытие") { route.arrival = responce.schedule[i].arrival; }
                    else {route.depart = responce.schedule[i].departure; } 
                    schedule.Add(route);
                }
                return schedule;
            }
            catch (Exception ex)
            {
                return schedule;
            }
        }
        public static List<string> stationID_get(string lat, string lon)
        {
            Auth auth = new Auth();
            List<string> schedule = new List<string>();
            //Адрес к которому отправляем запрос
            //URL to what we send the requests
            string api_point = $"https://api.rasp.yandex.net/v3.0/nearest_stations/?apikey={auth.key}&format=json&lat={lat}&lng={lon}&distance=50&lang=ru_RU&station_types=station";
            //Создаем новый клиент Rest
            //Creating a new rest client
            var client = new RestClient();

            //Настройки клиента Rest 
            //Settings of Rest client 
            //Чтобы получить расписание станции или маршута нам нужен ID станции, тут мы его получаем по координатам
            //To get schedule of the route or to get a scedule of station we need a station ID here we can get the id by coordinates
            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";

            //Отправляем запрос
            //Send the request
            var json = client.MakeRequest();
            //Парсим ответ
            //Parce the respoce
            dynamic responce = JObject.Parse(json);

            Console.WriteLine(responce.ToString());
            return schedule;
        }
        public static List<Route> route_time_get(string date, string from, string to)
        {
            Auth auth = new Auth();
            //Лист для возврата расписания
            List<Route> Route_list = new List<Route>();
            //Адрес к которому отправляем запрос
            string api_point = $"https://api.rasp.yandex.net/v3.0/search/?apikey={auth.key}&format=json&from={from}&to={to}&lang=ru_RU&page=1&date={date}";
            //Создаем новый клиент Rest
            var client = new RestClient();
            //Schedule of route
            //Настройки клиента 
            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";

            //Отправляем запрос
            var json = client.MakeRequest();
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = responce.ToString().Length;
            //auth.Uid = stuff.uid.ToString();
            Console.WriteLine(responce.segments[0].thread.uid);
            auth.uid = responce.segments[0].thread.uid;
            try
            {
                //В цикле собираем все записи и переносим их в вид списка
                for (int i = 0; i < length; i++)
                {
                    var route = new Route();
                    route.depart = responce.segments[i].departure;
                    //Console.WriteLine(stuff.segments[i].thread.uid);
                    route.arrival = responce.segments[i].arrival;
                    route.title = responce.segments[i].thread.short_title;
                    Route_list.Add(route);
                }
                return Route_list;
                
            }
            catch (Exception ex)
            {
                return Route_list;
            }
        }
        public static void route_get_station() 
        {
            Auth auth = new Auth();
            Console.WriteLine(auth.uid);
            //Лист для возврата расписания
            List<Route> Route_list = new List<Route>();
            //Адрес к которому отправляем запрос
            string api_point = $"https://api.rasp.yandex.net/v3.0/thread/?apikey={auth.key}&format=json&uid={auth.uid}&lang=ru_RU&show_systems=all";
            //Создаем новый клиент Rest
            var client = new RestClient();
            //Schedule of route
            //Настройки клиента 
            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";

            //Отправляем запрос
            var json = client.MakeRequest();
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = 31;
            for (int i = 0; i < length; i++) 
            {
                string code = responce.stops[i].station.code;
                string title = responce.stops[i].station.title;
                DB.Write_station(title,code);
            }
            
        }
    }
}
