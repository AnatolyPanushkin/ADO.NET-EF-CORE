using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class MinionVillain
    {
        public int MinionId { get; set; }
        public int VillainId { get; set; }

        public virtual Minion Minion { get; set; }
        public virtual Villain Villain { get; set; }
    }
}
