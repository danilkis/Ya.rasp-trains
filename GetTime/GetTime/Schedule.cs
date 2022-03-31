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
            private string uid;
            private string api_key;
            public string Uid
            {
                get { return this.uid; }
                set { this.uid = value; }
            }

            public string key
            {
                get { return "API KEY HERE"; }
                set { }
            }
        }
        public static List<string> station_timetable_get(string date, string station, bool from_station)
        //This is the json parcer so it basicall gets an answer from the api and gets the needed data
        {
            Auth auth = new Auth();
            //Лист для возврата расписания
            //A list to return the schedule in list
            List<string> schedule = new List<string>();
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
                    if (from_station == true)
                    {
                        if (responce.schedule[i].direction != "прибытие")
                        {
                            string time = responce.schedule[i].departure;
                            //Время отправления
                            //departure time
                            string direction = responce.schedule[i].short_title;
                            //Направление поезда
                            //train direction
                            string message = $"Отправление --> {direction}  {time}";
                            //Сообщение с направлением и временем (!!ПЕРЕРАБОТАТЬ!!)
                            //message with direction and time (here it is going from the station)
                            schedule.Add(message);
                        }
                    }
                    else
                    {
                        if (responce.schedule[i].arrival != null)
                        {
                            string time = responce.schedule[i].arrival;
                            string message = $"Прибытие <-- {time}";
                            //here it goes to station
                            schedule.Add(message);
                        }
                    }
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
        public static List<string> route_time_get(string date, string from, string to)
        {
            Auth auth = new Auth();
            //Лист для возврата расписания
            List<string> schedule = new List<string>();
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
            try
            {
                //В цикле собираем все записи и переносим их в вид списка
                //I better use foreach here and not for
                for (int i = 0; i < length; i++)
                {
                    string depart = responce.segments[i].departure;
                    //Console.WriteLine(stuff.segments[i].thread.uid);
                    string arrival = responce.segments[i].arrival;
                    string title1 = responce.segments[i].thread.short_title;
                    string message = $"(Маршрут {title1})Отправление в :{depart} Прибытие в {arrival}";
                    schedule.Add(message);
                }
                return schedule;
            }
            catch (Exception ex)
            {
                return schedule;
            }
        }
    }
}
