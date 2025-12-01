using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTaxiApp
{
    public class Client
    {
        public string LastName { get; set; }
        public List<Trip> Trips { get; set; }

        public Client()
        {
            Trips = new List<Trip>();
        }

        public Client(string lastName) : this()
        {
            LastName = lastName;
        }

        public decimal GetTotalCost()
        {
            return Trips.Sum(t => t.Cost);
        }
    }
}
