using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_paw_2.Entities
{
    [Serializable]

    internal class All
    {
        public int Id { get; set; }
        public List<Client> Clients { get; set; }
        public List<Insurance> Insurances { get; set; }
        public List<Event> Events { get; set; }

        public All() 
        {
            Clients = new List<Client>();
            Insurances = new List<Insurance>();
            Events = new List<Event>();
        }

    }
}
