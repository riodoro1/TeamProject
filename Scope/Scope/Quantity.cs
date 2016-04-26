using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Scope
{
    public class Quantity : object
    {
        private double value;
        private string unit;

        private string SIPrefix(ref double value)
        {
            double[] multipliers = { 1000000000000, 1000000000, 1000000, 1000, 1.0, 0.001, 0.000001, 0.000000001, 0.000000000001};
            string[] prefixes = { "T", "G", "M", "k", "", "m", "µ", "n", "p" };

            int prefixIndex = 0;
            for (; prefixIndex < 8; prefixIndex++)
            {
                double valueScaled = value / multipliers[prefixIndex];
                if ( valueScaled >= 0.999 ) //1.0 here vould round terribly
                {
                    break;
                }
            }
            if (prefixIndex == 9)
                prefixIndex = 8;

            value = value / multipliers[prefixIndex];
            return prefixes[prefixIndex];
        }

        public Quantity(double _value, string _unit)
        {
            value = _value;
            unit = _unit;
        }


        public override string ToString()
        {
            double scaledValue = value;
            string prefix = SIPrefix(ref scaledValue);
            return scaledValue.ToString("F1") + "" + prefix + unit;
        }
    }

    public class DoubleAndStringToQuantityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            double? val = (value[0] as double?);
            string unit = (value[1] as string);
            string postfix = (parameter as string);

            if (val.HasValue && unit != null)
                return (new Quantity(val.Value, unit)).ToString() + (postfix == null? "": postfix);
            else
                throw new InvalidCastException("Could not convert from values");
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

}
