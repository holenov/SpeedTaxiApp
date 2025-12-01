using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTaxiApp
{
    public class Tariff : IPriceCalculator
    {
        public string Category { get; set; }
        public decimal PricePerKm { get; set; }

        public Tariff() { }

        public Tariff(string category, decimal pricePerKm)
        {
            Category = category;
            PricePerKm = pricePerKm;
        }

        public decimal CalculatePrice(decimal distance)
        {
            return distance * PricePerKm;
        }

        public override string ToString()
        {
            return $"{Category}: {PricePerKm} грн/км";
        }
    }
}
