using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scope
{
    class Quantity : Object
    {
        private Double value;
        private String unit;

        private String SIPrefix(ref Double value)
        {
            Double[] multipliers = { 1000000000000, 1000000000, 1000000, 1000, 1.0, 0.001, 0.000001, 0.000000001, 0.000000000001};
            String[] prefixes = { "T", "G", "M", "k", "", "m", "µ", "n", "p" };

            int prefixIndex = 0;
            for (; prefixIndex < 8; prefixIndex++)
            {
                Double valueScaled = value / multipliers[prefixIndex];
                if ( valueScaled >= 1.0 )
                {
                    break;
                }
            }
            if (prefixIndex == 9)
                prefixIndex = 8;

            value = value / multipliers[prefixIndex];
            return prefixes[prefixIndex];
        }

        public Quantity(Double _value, String _unit)
        {
            value = _value;
            unit = _unit;
        }


        public override string ToString()
        {
            Double scaledValue = value;
            String prefix = SIPrefix(ref scaledValue);
            return scaledValue.ToString("F1") + " " + prefix + unit;
        }
    }
}
