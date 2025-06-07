using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_paw_2.Entities
{
    public class Event
    {
        public int Event_Id { get; set; }
        public string Name { get; set;}
        public DateTime Date { get; set; }
        public int Client_Id { get; set; }
        public int Insurance_Id { get; set; }

        public Event()
        {
            Date = DateTime.Now;
        }

        public Event(int event_Id, int client_Id, int insurance_Id, string name, DateTime date)
        {
            Event_Id = event_Id;
            Name = name;
            Date = date;
            Client_Id = client_Id;
            Insurance_Id = insurance_Id;
        }
    }
}
