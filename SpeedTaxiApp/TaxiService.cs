using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTaxiApp
{
    public class TaxiService
    {
        public Dictionary<string, Tariff> Tariffs { get; set; }
        public List<Client> Clients { get; set; }

        public TaxiService()
        {
            Tariffs = new Dictionary<string, Tariff>();
            Clients = new List<Client>();
        }

        public void AddTariff(Tariff tariff)
        {
            Tariffs[tariff.Category] = tariff;
        }

        public void RegisterClient(string lastName)
        {
            if (!Clients.Any(c => c.LastName == lastName))
                Clients.Add(new Client(lastName));
        }

        public void AddTrip(string clientLastName, string category, decimal distance)
        {
            var client = Clients.FirstOrDefault(c => c.LastName == clientLastName);
            if (client == null)
                throw new Exception("Клієнта не знайдено!");

            if (!Tariffs.ContainsKey(category))
                throw new Exception("Тариф не знайдено!");

            decimal cost = Tariffs[category].CalculatePrice(distance);

            client.Trips.Add(new Trip(category, distance, cost));
        }

        public decimal GetProfit()
        {
            return Clients.Sum(c => c.GetTotalCost());
        }
    }
}
