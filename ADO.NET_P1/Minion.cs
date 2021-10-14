using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class Minion
    {
        public Minion()
        {
            MinionVillains = new HashSet<MinionVillain>();
        }

        public Minion(string name, int age, int town)
        {
            Name = name;
            Age = age;
            TownId = town;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int? TownId { get; set; }

        public virtual Town Town { get; set; }
        public virtual ICollection<MinionVillain> MinionVillains { get; set; }
    }
}
