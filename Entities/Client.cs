using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace proiect_paw_2.Entities
{
    [Serializable]

    public class Client
    {
        public int Client_Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public Client() 
        {
            Birthday = DateTime.Now;
        }

        public Client(int client_Id, string name, DateTime birthday)
        {
            Client_Id = client_Id;
            Name = name;
            Birthday = birthday;
        }
    }
}
