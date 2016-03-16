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

namespace Scope
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Point[] points = new Point[1801];

            for (int x = 0; x <= 1800; x++)
            {
                points[x] = new Point(((double)x) / 50.0, (Math.Sin((double)x * (4 * Math.PI / 1800.0)) * 2.0));
            }

            Signal signal = new TimeDomainSignal(points, Colors.Red);
            SignalDisplay.AddSignal(signal);
            ((Button)sender).IsEnabled = false;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Point[] points = new Point[501];

            for (int x = 0; x <= 500; x++)
            {
                points[x] = new Point(((double)x) / 50.0 - 5.0, (Math.Cos((double)x * (2 * Math.PI / 500.0)) * 4.0));
            }

            Signal signal = new TimeDomainSignal(points, Colors.Green);
            SignalDisplay.AddSignal(signal);
            ((Button)sender).IsEnabled = false;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SignalDisplay.ClearSignals();
            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
        }
    }
}
