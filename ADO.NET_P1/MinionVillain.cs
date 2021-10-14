using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class MinionVillain
    {
        public MinionVillain(int minionId, int villainId)
        {
            MinionId=minionId;
            VillainId = villainId;
        }

        public int MinionId { get; set; }
        public int VillainId { get; set; }

        public virtual Minion Minion { get; set; }
        public virtual Villain Villain { get; set; }
    }
}
