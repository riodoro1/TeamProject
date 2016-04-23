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
    /// Interaction logic for SignalListItem.xaml
    /// </summary>
    public partial class SignalListItem : UserControl
    {
        public Signal Signal
        {
            get
            {
                return (Signal)GetValue(SignalProperty);
            }
            set
            {
                SetValue(SignalProperty, value);
            }
        }

        public static readonly DependencyProperty SignalProperty = DependencyProperty.Register("Signal", typeof(Signal), typeof(SignalListItem), new PropertyMetadata(OnSignalChanged));

        private static void OnSignalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SignalListItem item = (obj as SignalListItem);
            item.DataContext = item.Signal;
        }

        public SignalListItem()
        {
            InitializeComponent();
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if ( e.NewValue.HasValue )
            {
                Signal.Color = e.NewValue.Value;
                if ( Signal.Display != null )
                {
                    Signal.Display.RefreshDisplay();
                }
            }
        }

        private void DisableTextBox(TextBox box)
        {
            box.BorderThickness = new Thickness(0);
            box.Background = new SolidColorBrush(Colors.Transparent);
            box.Cursor = Cursors.Arrow;
            box.Focusable = false;
        }
        private void EnableTextBox(TextBox box)
        {
            box.BorderThickness = new Thickness(1);
            box.Background = new SolidColorBrush(Colors.White);
            box.Cursor = Cursors.IBeam;
            box.Focusable = true;
        }
        private void SaveName()
        {
            Signal.Name = NameTextBox.Text;
        }

        private void NameTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox box = (sender as TextBox);
            EnableTextBox(box);
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (sender as TextBox);
            DisableTextBox(box);
            SaveName();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (sender as TextBox);
            if ( e.Key == Key.Return )
            {
                DisableTextBox(box);
                SaveName();
            }
        }
    }
}
