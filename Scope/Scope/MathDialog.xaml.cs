using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Scope
{
    /// <summary>
    /// Interaction logic for MathWindow.xaml
    /// </summary>
    public partial class MathDialog : Window
    {
        public static SignalOperator[] Operators = { new SignalSummator(), new SignalSubtractor(), new SignalMultiplier(), new SignalDivider() };

        public Signal Signal = null;

        public MathDialog(ObservableCollection<Signal> signals)
        {
            InitializeComponent();
            DataContext = signals;

            operatorBox.ItemsSource = Operators;
        }

        private void Validate()
        {
            if (operatorBox.SelectedItem == null)
            {
                addButton.IsEnabled = false;
                return;
            }

            SignalOperator signalOperator = (operatorBox.SelectedItem as SignalOperator);
            if (signalOperator.NumberOfOperands == 1)
            {
                if (firstOperandBox.SelectedItem == null)
                {
                    addButton.IsEnabled = false;
                    return;
                }
            }
            else
            {
                if (firstOperandBox.SelectedItem == null || secondOperandBox.SelectedItem == null)
                {
                    addButton.IsEnabled = false;
                    return;
                }
            }

            addButton.IsEnabled = true;
        }

        private void operatorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SignalOperator signalOperator = (operatorBox.SelectedItem as SignalOperator);

            if (signalOperator.NumberOfOperands == 1)
            { 
                secondOperandBox.IsEnabled = false;
                firstOperandBox.IsEnabled = true;
            }
            else
            {
                secondOperandBox.IsEnabled = true;
                firstOperandBox.IsEnabled = true;
            }

            Validate();
        }

        private void firstOperandBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Validate();
        }

        private void secondOperandBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Validate();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            Color color = colorColorPicker.SelectedColor.Value;
            SignalOperator signalOperator = (operatorBox.SelectedItem as SignalOperator);

            if ( firstOperandBox.SelectedItem == null )
            {
                this.DialogResult = false;
            }

            if ( signalOperator.NumberOfOperands == 1 )
            {
                Signal firstOperand = (firstOperandBox.SelectedItem as Signal);
                Signal = signalOperator.Result(firstOperand);
            }
            else
            {
                Signal firstOperand = (firstOperandBox.SelectedItem as Signal);
                Signal secondOperand = (secondOperandBox.SelectedItem as Signal);
                Signal = signalOperator.Result(firstOperand, secondOperand);
            }

            if (Signal == null)
            {
                this.DialogResult = false;
            }
            else
            {
                Signal.Name = name;
                Signal.Color = color;
                this.DialogResult = true;
            }
        }
    }

    public class ColorToSolidColorBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
