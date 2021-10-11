using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class Villain
    {
        public Villain()
        {
            MinionVillains = new HashSet<MinionVillain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? EvilnessFactorId { get; set; }

        public virtual EvilnessFactor EvilnessFactor { get; set; }
        public virtual ICollection<MinionVillain> MinionVillains { get; set; }
    }
}
