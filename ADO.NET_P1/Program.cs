using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Win32.SafeHandles;

namespace ADO.NET_P1
{
    class Program : DbContext
    {
        static string connectionString = "Server=.;Database=ADO.NET_P1;Trusted_Connection=True";
        static void Main(string[] args)
        {
            //SelectVillainsId(3);
            for (int i = 0; i < 40; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");
            //SelectMore3Minions();
            //SetMinion("d",4,"fsd");
            //SelectVillainsWhoHaveMoreThan3Minions();
            //Console.Write("Введите ID злодея:");
            //DeleteVillainById(int.Parse(Console.ReadLine() ?? String.Empty));
            Console.Write("Введите данные миньона:");
            string[] str = Console.ReadLine()?.Split(" ");
            SetMinion(str[0],int.Parse(str[1]),str[2]);
        }

        static void DeleteVillainById(int id)
        {
            using var context =new ADONET_P1Context();
            var villain = context.Villains.Where(e => e.Id == id).Select(e=>e).SingleOrDefault();
            if (villain==null)
            {
                Console.WriteLine($"Такой злодей не найден.");
                return;
            }
            int quantity = 0;
            foreach (var v in context.MinionVillains) {
                if (v.VillainId == villain.Id) {
                    context.MinionVillains.Remove(v);
                    quantity++;
                }
            }
            context.Remove(villain);
            context.SaveChanges();
            Console.WriteLine($"{villain.Name} был удален.");
            Console.WriteLine($"{quantity} миньонов освобождено.");
        }

        static void SetMinion(string name, int age,string town)
        {  
           
            var context = new ADONET_P1Context();
            using (context)
            {
                Console.Write("Villain: ");
                string villainName=Console.ReadLine();
                
                var towns = (from t in context.Towns where t.Name.Equals(town) select t).SingleOrDefault();
                if (towns == null)
                {
                    var newTown = new Town(town);
                    context.Towns.Add(newTown);
                    context.SaveChanges();
                    Console.WriteLine($"Город {newTown.Name} был добавлен в базу данных.");
                  
                }

                var town2 = context.Towns.Where(e => e.Name.Equals(town)).Select(e => e).ToArray();
                

                var villain = context.Villains.Where(e => e.Name.Equals(villainName)).Select(e => e).SingleOrDefault();
                if (villain==null)
                {
                    var newVillain = new Villain(villainName);
                    context.Villains.Add(newVillain);
                    context.SaveChanges();
                    Console.WriteLine($"Злодей {newVillain.Name}был добавлен в базу даных");
                    
                }
                
                var villain2 = context.Villains.Where(e => e.Name.Equals(villainName)).Select(e => e).ToArray();

               
                
                    var newMinion = new Minion(name, age, town2[0].Id);
                    context.Minions.Add(newMinion);
                    context.SaveChanges();
                
                    Console.WriteLine($"Успешно добавлен {newMinion.Name}, чтобы быть миньоном {villain2[0].Name}");

                    var newMinionVillain = new MinionVillain(newMinion.Id,villain2[0].Id);
                    context.MinionVillains.Add(newMinionVillain);
                

                context.SaveChanges();
                

            }
        }

        static void SelectVillainsId(int id)
        {
            var context = new ADONET_P1Context();
            using (context)
            {
                var view1 = from villain in context.Villains
                    where villain.Id == id
                    select new
                    {
                        villain.Name
                    };
                if (view1.ToArray().Length != 0)
                {
                    Console.WriteLine("Villain: {0}",view1.ToArray()[0].Name);
                    var view = from minion in context.Minions
                        join minionVillain in context.MinionVillains on minion.Id equals minionVillain.MinionId
                        where minionVillain.VillainId == id
                        select new
                        {
                            minion.Name, minion.Age
                        };
                    foreach (var VARIABLE in view)
                    {
                        Console.WriteLine($"{VARIABLE.Name} - {VARIABLE.Age}");
                    }
                }
                else
                {
                    Console.WriteLine($"No villain with ID {id} exist in the database ");
                }




            }

        }

    
        
        static void SelectMore3Minions()
        {
            var context = new ADONET_P1Context();
            using (context)
            {
                var view = from b in context.Bs
                    where b.TotalMinions > 3
                    select new
                    {
                        b.Name, b.TotalMinions
                    };
                foreach (var variable in view)
                {
                    Console.WriteLine("{0} - {1}",variable.Name,variable.TotalMinions);
                }
            }
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
