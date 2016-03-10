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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SignalPoint[] points = new SignalPoint[1600];

            for (int x = 1; x <= 1600; x++)
            {
                points[x - 1] = new SignalPoint(((double)x) / 100.0, (Math.Sin((double)x * (4 * Math.PI / 1600.0)) * 100.0));
            }

            Signal signal = new Signal(points, Colors.Red);
            SignalDisplay.AddSignal(signal);
            ((Button)sender).IsEnabled = false;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SignalPoint[] points = new SignalPoint[800];

            for (int x = 1; x <= 800; x++)
            {
                points[x - 1] = new SignalPoint(((double)x) / 100.0 + 4, (Math.Cos((double)x * (2 * Math.PI / 800.0)) * 100.0));
            }

            Signal signal = new Signal(points, Colors.Green);
            SignalDisplay.AddSignal(signal);
            ((Button)sender).IsEnabled = false;
        }
    }
}
