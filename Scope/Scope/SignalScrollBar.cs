using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scope.Controls
{
    public class SignalScrollBar : ScrollBar
    {
        static SignalScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalScrollBar), new FrameworkPropertyMetadata(typeof(SignalScrollBar)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen p = new Pen(new SolidColorBrush(Colors.Red), 0.5);

            drawingContext.DrawLine(p, new Point(0, ActualHeight / 2), new Point(ActualWidth, ActualHeight / 2));

            base.OnRender(drawingContext);
        }
    }
}
