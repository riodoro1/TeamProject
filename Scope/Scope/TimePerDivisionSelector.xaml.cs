using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scope.Controls
{
    /// <summary>
    /// Interaction logic for TimePerDivisionSelector.xaml
    /// </summary>
    public partial class TimePerDivisionSelector : UserControl
    {
        private double[] values = { 0.000001,  0.000002,  0.000005,
                                    0.00001,   0.00002,   0.00005,
                                    0.0001,    0.0002,    0.0005,
                                    0.001,     0.002,     0.005,
                                    0.01,      0.02,      0.05,
                                    0.1,       0.2,       0.5,
                                    1,         2,         5};
        private int currentValue = 0;

        public event RoutedPropertyChangedEventHandler<double> ValueChanged;

        public TimePerDivisionSelector()
        {
            InitializeComponent();
            valueTextBlock.Text = new Quantity(values[currentValue], "s/div").ToString();
        }

        private void valueChanged()
        {
            valueTextBlock.Text = new Quantity(values[currentValue], "s/div").ToString();
            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(0.0, values[currentValue]);
            ValueChanged(this, args);
        }

        public void setNearestValueTo(double value)
        {
            int closestValue = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if ( Math.Abs(values[i] - value) < Math.Abs(values[closestValue] - value))
                {
                    closestValue = i;
                }
            }
            currentValue = closestValue;
            valueChanged();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if ( currentValue != values.Length - 1)
            {
                currentValue++;
            }
            
            valueChanged();
        }

        private void DnButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentValue != 0 )
            {
                currentValue--;
            }
            
            valueChanged();
        }
    }
}
