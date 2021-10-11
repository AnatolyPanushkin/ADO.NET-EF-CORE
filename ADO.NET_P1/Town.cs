using System;
using System.Collections.Generic;

#nullable disable

namespace ADO.NET_P1
{
    public partial class Town
    {
        public Town()
        {
            Minions = new HashSet<Minion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CountryCode { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
        public virtual ICollection<Minion> Minions { get; set; }
    }
}
