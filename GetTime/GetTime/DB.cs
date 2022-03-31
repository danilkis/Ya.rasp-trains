using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace GetTime
{
    class DB
    {
        public struct Station
        {
            public string title;
            public string code;
        };
        public static void Write_station(string title, string code)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=D:\Studio 2022\C#\GetTime\GetTime\stations.db; Version=3;"))
            {
                string commandText = "INSERT INTO [base] ([code], [title]) VALUES(@code, @title)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@code", code);
                Command.Parameters.AddWithValue("@title", title);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }
        public static Station Find_station_code(string title)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=D:\Studio 2022\C#\GetTime\GetTime\stations.db; Version=3;"))
            {
                string commandText = "SELECT * FROM [base]";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                DataTable data = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(Command);
                adapter.Fill(data);
                Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
                var res_station = new Station();
                foreach (DataRow row in data.Rows)
                {
                    if (title == row.Field<string>("title"))
                    {
                        res_station.code = row.Field<string>("code");
                        res_station.title = row.Field<string>("title");
                        return res_station;
                    }
                }

                return res_station;
            }
        }
    }
}
