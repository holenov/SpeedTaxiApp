using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTaxiApp
{
    public class Trip
    {
        public string Category { get; set; }
        public decimal Distance { get; set; }
        public decimal Cost { get; set; }

        public Trip() { }

        public Trip(string category, decimal distance, decimal cost)
        {
            Category = category;
            Distance = distance;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"{Category}, {Distance} км, {Cost} грн";
        }
    }
}
