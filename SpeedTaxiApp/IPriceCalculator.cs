using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTaxiApp
{
    public interface IPriceCalculator
    {
        decimal CalculatePrice(decimal distance);
    }
}
