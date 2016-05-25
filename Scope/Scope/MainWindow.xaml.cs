using Microsoft.Win32;
using Scope.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            SignalGeneratorDialog dialog = new SignalGeneratorDialog();
            if(dialog.ShowDialog() == true)
            {
                SignalDisplay.AddSignal(dialog.Signal);
                if ( SignalDisplay.Signals.Count == 1 )
                {
                    TimePerDivisionSelector.setNearestValueTo(dialog.Signal.Duration / SignalDisplay.majorHorizontalDivisions); //if we added the first signal change the signal display to fully show it
                    SignalDisplay.StartTime = dialog.Signal.FirstXValue;
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Signal selectedItem = (SignalsListBox.SelectedItem as Signal);
            SignalDisplay.RemoveSignal(selectedItem);
        }

        private void mathButton_Click(object sender, RoutedEventArgs e)
        {
            Signal selectedSignal = (SignalsListBox.SelectedItem as Signal);

            MathDialog dialog = new MathDialog(SignalDisplay.Signals, selectedSignal);
            if (dialog.ShowDialog() == true)
            {
                SignalDisplay.AddSignal(dialog.Signal);
            }
        }

        private void SignalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( SignalsListBox.SelectedIndex != -1 )
            {
                DeleteButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
            else
            {
                DeleteButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "CSV Files (*.csv)|*.csv";
            openDialog.Multiselect = true;

            if ( openDialog.ShowDialog() == true )
            {
                string[] files = openDialog.FileNames;

                foreach (string file in files)
                {
                    SignalOpener opener = new SignalOpener(file);

                    try
                    {
                        Signal signal = opener.readSignal();
                        SignalDisplay.AddSignal(signal);
                        if (SignalDisplay.Signals.Count == 1)
                        {
                            TimePerDivisionSelector.setNearestValueTo(signal.Duration / SignalDisplay.majorHorizontalDivisions); //if we added the first signal change the signal display to fully show it
                            SignalDisplay.StartTime = signal.FirstXValue;
                        }
                    }
                    catch ( FileLoadException )
                    {
                        //file could not be open, maybe an error?
                    }
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Signal selectedSignal = ( SignalsListBox.SelectedItem as Signal );
            if (selectedSignal == null)
                return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveDialog.FileName = selectedSignal.Name;

            if ( saveDialog.ShowDialog() == true )
            {
                SignalSaver saver = new SignalSaver(selectedSignal);
                saver.SaveAt(saveDialog.FileName);
            }
        }
    }
}
