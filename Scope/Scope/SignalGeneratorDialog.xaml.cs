using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignalGeneratorDialog.xaml
    /// </summary>
    public partial class SignalGeneratorDialog : Window
    {
        public TimeDomainSignal Signal = null;

        public SignalGeneratorDialog()
        {
            InitializeComponent();
        }

        private void TextBoxValidateDouble(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            String name = nameTextBox.Text;
            Color color = colorColorPicker.SelectedColor.Value;

            bool parseResult = true;

            Double frequency;
            if (!Double.TryParse(frequencyTextBox.Text, out frequency) || frequency <= 0)
                parseResult = false;

            Double amplitude;
            if(!Double.TryParse(amplitudeTextBox.Text, out amplitude) || amplitude <= 0)
                parseResult = false;

            Double dcOffset;
            if(!Double.TryParse(dcOffsetTextBox.Text, out dcOffset))
                parseResult = false;

            Double startTime;
            if (!Double.TryParse(startTimeTextBox.Text, out startTime))
                 parseResult = false;

            Double duration;
            if(!Double.TryParse(durationTextBox.Text, out duration))
                parseResult = false;

            SignalGenerator generator = new SineWaveGenerator(frequency, amplitude, dcOffset);

            Signal = generator.GenerateSignal(startTime, duration);
            Signal.Name = name;
            Signal.Color = color;
            this.DialogResult = parseResult;
        }
    }
}
