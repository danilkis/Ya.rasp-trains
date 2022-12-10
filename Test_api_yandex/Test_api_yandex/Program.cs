using API.Yandex.Rasp;
using Newtonsoft.Json.Linq;
var auth = new Auth();
auth.key = ""; //Ваш ключ тут
DateTime data = DateTime.Today;
string ID1 = "s9602496"; 
string ID2 = "s9603134";
List<Route> station_sch;
station_sch = get.station_schedule(data, ID1,auth);
foreach (Route station in station_sch) 
{
    if (station.depart != null) { Console.WriteLine("---> " + station.depart); }
    else if (station.arrival != null)  { Console.WriteLine("<--- " + station.arrival); }
}
List<Route> route1 = get.route(data, ID1, ID2, auth);
foreach (Route route in route1)
{
    Console.WriteLine(route.title + "  " + route.depart + "  " + route.arrival);
}
List<Station> stations;
stations = get.stationID(auth);
foreach (Station station1 in stations) 
{
    Console.WriteLine(station1.code + "  " + station1.title);
}