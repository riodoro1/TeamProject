
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
    public class SignalDisplay : Canvas
    {
        #region Properties
        private bool Changed = true;
        private List<Signal> Signals;

        private int majorVerticalDivisions = 8;
        private int majorHorizontalDivisions
        {
            get
            {
                return Math.Min(Math.Max((int)Math.Round(ActualWidth / ActualHeight) * majorVerticalDivisions, 4), 50);
            }
        } //depending on width to height ratio

        public Double timePerDivision { get; set; } = 1.0d; //horizontal
        public Double unitsPerDivision { get; set; } = 1.0d; //vertical

        public Double StartTime { get; set; } = 0.0d;
        public Double EndTime
        {
            get
            {
                return StartTime + majorHorizontalDivisions * timePerDivision;
            }
        }
        public Color GraticuleColor { get; set; } = Colors.Gray;
        #endregion

        static SignalDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalDisplay), new FrameworkPropertyMetadata(typeof(SignalDisplay)));
        }

        public SignalDisplay()
        {
            Signals = new List<Signal>();
        }

        public void AddSignal(Signal signal)
        {
            Signals.Add(signal);
            Changed = true;
        }

        private void DrawGraticule(DrawingContext dc)
        {
            double centerY = ActualHeight / 2.0;
            double centerX = ActualWidth / 2.0;
            double deltaY = ActualHeight / majorVerticalDivisions / 5.0;   //5 minor divisions
            double deltaX = ActualWidth / majorHorizontalDivisions / 5.0;   //5 minor divisions

            Pen graticulePen = new Pen(new SolidColorBrush(GraticuleColor), 0.4);

            double x = 0.0;
            while (x <= ActualWidth)
            {
                dc.DrawLine(graticulePen, new Point(x, 0.0), new Point(x, ActualHeight));
                x += deltaX;
                
                for (int i = 0; i < 4 && x<=ActualWidth; i++)
                {
                    dc.DrawLine(graticulePen, new Point(x, centerY - deltaY / 2.0), new Point(x, centerY + deltaY / 2.0));
                    x += deltaX;
                }
            }

            double y = 0.0;
            while (y <= ActualHeight)
            {
                dc.DrawLine(graticulePen, new Point(0.0, y), new Point(ActualWidth, y));
                y += deltaY;
                for (int i = 0; i < 4 && y <= ActualHeight; i++)
                {
                    dc.DrawLine(graticulePen, new Point(centerX - deltaX / 2.0, y), new Point(centerX + deltaX / 2.0, y));
                    y += deltaY;
                }
            }
        }

        private void DrawSignals(DrawingContext dc)
        {
            if (Signals.Count == 0)
                return;

        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (Changed)
            {
                Changed = false;
                DrawGraticule(dc);
                foreach (Signal signal in Signals)
                {
                    DrawSignals(dc);
                }
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Changed = true;
            this.InvalidateVisual();
        }
    }
}