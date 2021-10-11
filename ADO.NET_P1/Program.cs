using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;

namespace ADO.NET_P1
{
    class Program
    {
        static string connectionString = "Server=.;Database=ADO.NET_P1;Trusted_Connection=True";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SelectVillainsWhoHaveMoreThan3Minions();
        }

        static void SelectVillainAndHisMinions(int id)
        {

        }
        static void SelectVillainsWhoHaveMoreThan3Minions()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using (sqlConnection)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM b WHERE TotalMinions>3" +
                "ORDER BY TotalMinions DESC", sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetString(0)} - {reader.GetInt32(1)}");
                    }
                }
            }
        }

        static void TestSelectVillains()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using (sqlConnection)
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM MinionVillain", sqlConnection);
                int count = (int)command.ExecuteScalar();
                Console.WriteLine("MinionVillain count:{0}", count);
            }
        }

        static void TestGroupBy()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using (sqlConnection)
            {
                SqlCommand command = new SqlCommand("SELECT e.VillainId, FROM MinionVillain AS e GROUP  BY e.VillainId");
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string minion = (string)reader["VillainId"];
                        Console.WriteLine("minion id:{0}", minion);
                    }
                }
            }
        }
    }
}
