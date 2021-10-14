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
            
            //DeleteVillainById(int.Parse(Console.ReadLine() ?? String.Empty));
            
            // Console.Write("Введите данные миньона:");
            // string[] str = Console.ReadLine()?.Split(" ");
            // SetMinion(str[0],int.Parse(str[1]),str[2]);
            
            IncreaseMinionsAgeById();
        }
        
        /// <summary>
        /// Функция для увеличения возраста миньонов 
        /// </summary>
        static void IncreaseMinionsAgeById()
        {
            //Вводим Id минионов, у которых хотим увеличить возраст
            int[] minionsId = Console.ReadLine()?.Split(" ").Select(int.Parse).ToArray();

            using var context = new ADONET_P1Context();
            
            //Ищем миньонов с нужными нам Id
            if (minionsId != null)
                for (int i = 0; i < minionsId.Length; i++)
                {
                    var minion = (from m in context.Minions where m.Id == minionsId[i] select m).SingleOrDefault();
                    if (minion != null) minion.Age++;
                }

            context.SaveChanges();
            
            //Выводим всех миньонов
            var minion2 = from m  in context.Minions select m;
            foreach (var v in  minion2)
            {
                Console.WriteLine($"{v.Name} {v.Age}");
            }
        }
        
        
        /// <summary>
        /// Функция поиска злодея по Id
        /// </summary>
        /// <param name="id">Id злодея</param>
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
        /// <summary>
        /// Функция добавления миньона
        /// </summary>
        /// <param name="name">Имя миньона</param>
        /// <param name="age">Возраст миньона</param>
        /// <param name="town">Название города миньона </param>
        static void SetMinion(string name, int age,string town)
        {
            var context = new ADONET_P1Context();
            using (context)
            {
                Console.Write("Villain: ");
                string villainName=Console.ReadLine();

                int townId = 0;
                var towns = (from t in context.Towns where t.Name.Equals(town) select t).SingleOrDefault();
                if (towns == null)
                {
                    var newTown = new Town(town);
                    context.Towns.Add(newTown);
                    context.SaveChanges();
                    Console.WriteLine($"Город {newTown.Name} был добавлен в базу данных.");
                    townId = newTown.Id;
                }

                string vlnName="";
                int vlnId = 0;
                var villain = context.Villains.Where(e => e.Name.Equals(villainName)).Select(e => e).SingleOrDefault();
                if (villain==null)
                {
                    var newVillain = new Villain(villainName);
                    context.Villains.Add(newVillain);
                    context.SaveChanges();
                    Console.WriteLine($"Злодей {newVillain.Name}был добавлен в базу даных");
                    vlnName = newVillain.Name;
                    vlnId = newVillain.Id;
                }
                
                var newMinion = new Minion(name, age, townId);
                context.Minions.Add(newMinion);
                context.SaveChanges();
                
                Console.WriteLine($"Успешно добавлен {newMinion.Name}, чтобы быть миньоном {vlnName}");

                var newMinionVillain = new MinionVillain(newMinion.Id,vlnId);
                context.MinionVillains.Add(newMinionVillain);

                context.SaveChanges();
            }
        }
        
        /// <summary>
        ///Функция поиска злодея и его миньонов по Id злодея 
        /// </summary>
        /// <param name="id">Id злодея</param>
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

    
        /// <summary>
        /// Функция поиска злодеев, имеющих более 3-х миньонов, с помощью LINQ запроса в стиле SQL
        /// </summary>
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
        /// <summary>
        /// Функция поиска злодеев, имеющих более 3-х миньонов, с помощью SQL запроса
        /// </summary>
        static void SelectVillainsWith3MinionsSQL()
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
