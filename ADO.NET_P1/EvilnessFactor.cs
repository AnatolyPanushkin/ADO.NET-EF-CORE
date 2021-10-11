using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class EvilnessFactor
    {
        public EvilnessFactor()
        {
            Villains = new HashSet<Villain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Villain> Villains { get; set; }
    }
}
